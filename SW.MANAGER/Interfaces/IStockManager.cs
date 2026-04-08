using SW.ENTITY.Dtos.Common;
using SW.ENTITY.Dtos.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.MANAGER.Interfaces
{
    public interface IStockManager
    {
        Task<BaseResponseDto> StockInAsync(StockInDto dto);
        Task<BaseResponseDto> StockOutAsync(StockOutDto dto);
    }
}
