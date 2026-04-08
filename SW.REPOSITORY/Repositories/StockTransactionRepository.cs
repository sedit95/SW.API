using Microsoft.EntityFrameworkCore;
using SW.ENTITY.Context;
using SW.ENTITY.Entities;
using SW.ENTITY.Enums;
using SW.REPOSITORY.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.REPOSITORY.Repositories
{
    public class StockTransactionRepository : IStockTransactionRepository
    {
        private readonly DatabaseContext _context;

        public StockTransactionRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task AddAsync(StockTransaction stockTransaction)
        {
            await _context.StockTransactions.AddAsync(stockTransaction);
        }

        public async Task<int> CountTodayInAsync(string companyId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.StockTransactions.CountAsync(x =>
                x.CompanyId == companyId &&
                !x.IsDeleted &&
                x.TransactionType == StockTransactionType.In &&
                x.TransactionDate.Date == today);
        }

        public async Task<int> CountTodayOutAsync(string companyId)
        {
            var today = DateTime.UtcNow.Date;

            return await _context.StockTransactions.CountAsync(x =>
                x.CompanyId == companyId &&
                !x.IsDeleted &&
                x.TransactionType == StockTransactionType.Out &&
                x.TransactionDate.Date == today);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
