using Microsoft.Data.SqlClient;
using SW.ENTITY.Dtos.Common;
using SW.ENTITY.Dtos.Product;
using SW.ENTITY.Entities;
using SW.MANAGER.Interfaces;
using SW.REPOSITORY.Interfaces;

namespace SW.MANAGER.Services;

public class ProductManager : IProductManager
{
    private readonly IProductRepository _productRepository;
    private readonly IProductCategoryRepository _productCategoryRepository;

    public ProductManager(
        IProductRepository productRepository,
        IProductCategoryRepository productCategoryRepository)
    {
        _productRepository = productRepository;
        _productCategoryRepository = productCategoryRepository;
    }

    public async Task<ProductDetailDto?> GetByIdAsync(int id, string companyId)
    {
        if (id <= 0 || string.IsNullOrWhiteSpace(companyId))
        {
            return null;
        }

        var product = await _productRepository.GetByIdAsync(id, companyId);

        if (product == null)
        {
            return null;
        }

        return new ProductDetailDto
        {
            Id = product.Id,
            CompanyId = product.CompanyId,
            ProductCode = product.ProductCode,
            ProductName = product.ProductName,
            ProductCategoryId = product.ProductCategoryId,
            CategoryName = product.ProductCategory?.CategoryName ?? string.Empty,
            UnitType = product.UnitType,
            MinimumStockLevel = product.MinimumStockLevel,
            CurrentStock = product.CurrentStock
        };
    }

    public async Task<PagedResponseDto<ProductListItemDto>> GetPagedAsync(ProductPagedRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyId))
        {
            return new PagedResponseDto<ProductListItemDto>
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur.",
                Data = new List<ProductListItemDto>(),
                TotalCount = 0,
                Page = request.Page <= 0 ? 1 : request.Page,
                PageSize = request.PageSize <= 0 ? 25 : request.PageSize,
                TotalPages = 0
            };
        }

        return await _productRepository.GetPagedAsync(request);
    }

    public async Task<BaseResponseDto> CreateAsync(ProductCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CompanyId))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.ProductCode))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün kodu zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.ProductName))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün adı zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.UnitType))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Birim tipi zorunludur.",
                StatusCode = 400
            };
        }

        if (dto.ProductCategoryId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir ürün kategorisi seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.MinimumStockLevel < 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Minimum stok seviyesi negatif olamaz.",
                StatusCode = 400
            };
        }

        var companyId = dto.CompanyId.Trim();
        var productCode = dto.ProductCode.Trim();
        var productName = dto.ProductName.Trim();
        var unitType = dto.UnitType.Trim();

        var category = await _productCategoryRepository.GetByIdAsync(dto.ProductCategoryId, companyId);

        if (category == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün kategorisi bulunamadı.",
                StatusCode = 404
            };
        }

        var existingProductByCode = await _productRepository.GetByCodeAsync(companyId, productCode);

        if (existingProductByCode != null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Aynı ürün koduna sahip başka bir ürün zaten mevcut.",
                StatusCode = 400
            };
        }

        var product = new Product
        {
            CompanyId = companyId,
            ProductCode = productCode,
            ProductName = productName,
            ProductCategoryId = dto.ProductCategoryId,
            UnitType = unitType,
            MinimumStockLevel = dto.MinimumStockLevel,
            CurrentStock = 0,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        try
        {
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Aynı ürün koduna sahip başka bir ürün zaten mevcut.",
                StatusCode = 400
            };
        }

        return new BaseResponseDto
        {
            IsSuccess = true,
            Message = "Ürün başarıyla oluşturuldu.",
            StatusCode = 200
        };
    }

    public async Task<BaseResponseDto> UpdateAsync(ProductUpdateDto dto)
    {
        if (dto.Id <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir Id değeri zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.CompanyId))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.ProductCode))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün kodu zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.ProductName))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün adı zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.UnitType))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Birim tipi zorunludur.",
                StatusCode = 400
            };
        }

        if (dto.ProductCategoryId <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir ürün kategorisi seçilmelidir.",
                StatusCode = 400
            };
        }

        if (dto.MinimumStockLevel < 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Minimum stok seviyesi negatif olamaz.",
                StatusCode = 400
            };
        }

        var existingProduct = await _productRepository.GetByIdAsync(dto.Id);

        if (existingProduct == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün bulunamadı.",
                StatusCode = 404
            };
        }

        if (existingProduct.CompanyId != dto.CompanyId)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Bu ürünü güncelleme yetkiniz bulunmamaktadır.",
                StatusCode = 403
            };
        }

        var category = await _productCategoryRepository.GetByIdAsync(dto.ProductCategoryId, dto.CompanyId);

        if (category == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün kategorisi bulunamadı.",
                StatusCode = 404
            };
        }

        var existingProductByCode = await _productRepository.GetByCodeAsync(dto.CompanyId, dto.ProductCode.Trim());

        if (existingProductByCode != null && existingProductByCode.Id != dto.Id)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Aynı ürün koduna sahip başka bir ürün zaten mevcut.",
                StatusCode = 400
            };
        }

        existingProduct.ProductCode = dto.ProductCode.Trim();
        existingProduct.ProductName = dto.ProductName.Trim();
        existingProduct.ProductCategoryId = dto.ProductCategoryId;
        existingProduct.UnitType = dto.UnitType.Trim();
        existingProduct.MinimumStockLevel = dto.MinimumStockLevel;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _productRepository.UpdateAsync(existingProduct);
            await _productRepository.SaveChangesAsync();
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Aynı ürün koduna sahip başka bir ürün zaten mevcut.",
                StatusCode = 400
            };
        }

        return new BaseResponseDto
        {
            IsSuccess = true,
            Message = "Ürün başarıyla güncellendi.",
            StatusCode = 200
        };
    }

    public async Task<BaseResponseDto> DeleteAsync(ProductDeleteDto dto)
    {
        if (dto.Id <= 0)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Geçerli bir Id değeri zorunludur.",
                StatusCode = 400
            };
        }

        if (string.IsNullOrWhiteSpace(dto.CompanyId))
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur.",
                StatusCode = 400
            };
        }

        var existingProduct = await _productRepository.GetByIdAsync(dto.Id);

        if (existingProduct == null)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Ürün bulunamadı.",
                StatusCode = 404
            };
        }

        if (existingProduct.CompanyId != dto.CompanyId)
        {
            return new BaseResponseDto
            {
                IsSuccess = false,
                Message = "Bu ürünü silme yetkiniz bulunmamaktadır.",
                StatusCode = 403
            };
        }

        existingProduct.IsDeleted = true;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        await _productRepository.UpdateAsync(existingProduct);
        await _productRepository.SaveChangesAsync();

        return new BaseResponseDto
        {
            IsSuccess = true,
            Message = "Ürün başarıyla silindi.",
            StatusCode = 200
        };
    }
}