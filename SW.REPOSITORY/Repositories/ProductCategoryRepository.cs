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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly DatabaseContext _context;

        public ProductCategoryRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<ProductCategory?> GetByIdAsync(int id, string companyId)
        {
            return await _context.ProductCategories
                .FirstOrDefaultAsync(x => x.Id == id && x.CompanyId == companyId && !x.IsDeleted);
        }
    }
}
