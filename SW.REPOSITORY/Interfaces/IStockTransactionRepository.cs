using SW.ENTITY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.REPOSITORY.Interfaces
{
    public interface IStockTransactionRepository
    {
        Task AddAsync(StockTransaction stockTransaction);
        Task<int> CountTodayInAsync(string companyId);
        Task<int> CountTodayOutAsync(string companyId);
        Task SaveChangesAsync();
    }
}
