using Microsoft.EntityFrameworkCore;
using SW.ENTITY.Context;
using SW.ENTITY.Entities;
using SW.REPOSITORY.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.REPOSITORY.Repositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly DatabaseContext _context;

        public WarehouseRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Warehouse?> GetByIdAsync(int id, string companyId)
        {
            return await _context.Warehouses
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId && !x.IsDeleted);
        }

        public async Task<int> CountAsync(string companyId)
        {
            return await _context.Warehouses
                .CountAsync(x => x.CompanyId == companyId && !x.IsDeleted);
        }
    }
}
