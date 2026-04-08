using SW.ENTITY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.REPOSITORY.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<Warehouse?> GetByIdAsync(int id, string companyId);
        Task<int> CountAsync(string companyId);
    }
}
