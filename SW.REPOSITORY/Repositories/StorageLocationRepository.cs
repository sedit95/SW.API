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
    public class StorageLocationRepository : IStorageLocationRepository
    {
        private readonly DatabaseContext _context;

        public StorageLocationRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<StorageLocation?> GetByIdAsync(int id, string companyId)
        {
            return await _context.StorageLocations
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId && !x.IsDeleted);
        }

        public async Task<StorageLocation?> GetByIdAsync(int id, int warehouseId, string companyId)
        {
            return await _context.StorageLocations
                .FirstOrDefaultAsync(x =>
                    x.Id == id &&
                    x.WarehouseId == warehouseId &&
                    x.CompanyId == companyId &&
                    !x.IsDeleted);
        }

        public async Task<int> CountAsync(string companyId)
        {
            return await _context.StorageLocations
                .CountAsync(x => x.CompanyId == companyId && !x.IsDeleted);
        }
    }
}
