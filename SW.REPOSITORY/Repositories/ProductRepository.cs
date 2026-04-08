using Microsoft.EntityFrameworkCore;
using SW.ENTITY.Context;
using SW.ENTITY.Dtos.Common;
using SW.ENTITY.Dtos.Product;
using SW.ENTITY.Entities;
using SW.REPOSITORY.Interfaces;

namespace SW.REPOSITORY.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly DatabaseContext _context;

    public ProductRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        return await _context.Products
            .Include(x => x.ProductCategory)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<Product?> GetByIdAsync(int id, string companyId)
    {
        return await _context.Products
            .Include(x => x.ProductCategory)
            .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId && !x.IsDeleted);
    }

    public async Task<Product?> GetByCodeAsync(string companyId, string productCode)
    {
        return await _context.Products
            .FirstOrDefaultAsync(x => x.CompanyId == companyId && x.ProductCode == productCode && !x.IsDeleted);
    }

    public async Task<PagedResponseDto<ProductListItemDto>> GetPagedAsync(ProductPagedRequestDto request)
    {
        var query = _context.Products
            .AsNoTracking()
            .Include(x => x.ProductCategory)
            .Where(x => x.CompanyId == request.CompanyId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(request.SearchText))
        {
            var searchText = request.SearchText.Trim().ToLower();

            query = query.Where(x =>
                x.ProductCode.ToLower().Contains(searchText) ||
                x.ProductName.ToLower().Contains(searchText) ||
                x.ProductCategory.CategoryName.ToLower().Contains(searchText));
        }

        if (request.ProductCategoryId.HasValue && request.ProductCategoryId.Value > 0)
        {
            query = query.Where(x => x.ProductCategoryId == request.ProductCategoryId.Value);
        }

        if (request.IsCriticalStock.HasValue)
        {
            if (request.IsCriticalStock.Value)
            {
                query = query.Where(x => x.CurrentStock <= x.MinimumStockLevel);
            }
            else
            {
                query = query.Where(x => x.CurrentStock > x.MinimumStockLevel);
            }
        }

        var totalCount = await query.CountAsync();

        var page = request.Page <= 0 ? 1 : request.Page;
        var pageSize = request.PageSize <= 0 ? 25 : request.PageSize;

        var data = await query
            .OrderBy(x => x.ProductName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(x => new ProductListItemDto
            {
                Id = x.Id,
                ProductCode = x.ProductCode,
                ProductName = x.ProductName,
                CategoryName = x.ProductCategory.CategoryName,
                UnitType = x.UnitType,
                MinimumStockLevel = x.MinimumStockLevel,
                CurrentStock = x.CurrentStock
            })
            .ToListAsync();

        return new PagedResponseDto<ProductListItemDto>
        {
            IsSuccess = true,
            Data = data,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Message = "Ürünler başarıyla listelendi."
        };
    }

    public async Task AddAsync(Product product)
    {
        await _context.Products.AddAsync(product);
    }

    public async Task UpdateAsync(Product product)
    {
        _context.Entry(product).State = EntityState.Modified;
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountAsync(string companyId)
    {
        return await _context.Products.CountAsync(x => x.CompanyId == companyId && !x.IsDeleted);
    }

    public async Task<int> CountCriticalStockAsync(string companyId)
    {
        return await _context.Products.CountAsync(x =>
            x.CompanyId == companyId &&
            !x.IsDeleted &&
            x.CurrentStock <= x.MinimumStockLevel);
    }

    public async Task<int> SumCurrentStockAsync(string companyId)
    {
        return await _context.Products
            .Where(x => x.CompanyId == companyId && !x.IsDeleted)
            .SumAsync(x => x.CurrentStock);
    }
}