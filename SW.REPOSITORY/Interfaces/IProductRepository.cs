using SW.ENTITY.Dtos.Common;
using SW.ENTITY.Dtos.Product;
using SW.ENTITY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.REPOSITORY.Interfaces
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByIdAsync(int id, string companyId);
        Task<Product?> GetByCodeAsync(string companyId, string productCode);
        Task<PagedResponseDto<ProductListItemDto>> GetPagedAsync(ProductPagedRequestDto request);
        Task<int> CountAsync(string companyId);
        Task<int> CountCriticalStockAsync(string companyId);
        Task<int> SumCurrentStockAsync(string companyId);
        Task AddAsync(Product product);
        Task UpdateAsync(Product product);
        Task SaveChangesAsync();
    }
}
