using SW.ENTITY.Dtos.Common;
using SW.ENTITY.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.MANAGER.Interfaces
{
    public interface IProductManager
    {
        Task<ProductDetailDto?> GetByIdAsync(int id, string companyId);
        Task<PagedResponseDto<ProductListItemDto>> GetPagedAsync(ProductPagedRequestDto request);
        Task<BaseResponseDto> CreateAsync(ProductCreateDto dto);
        Task<BaseResponseDto> UpdateAsync(ProductUpdateDto dto);
        Task<BaseResponseDto> DeleteAsync(ProductDeleteDto dto);
    }
}
