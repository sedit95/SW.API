using SW.ENTITY.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Entities
{
    public class StockTransaction : BaseEntity
    {
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int StorageLocationId { get; set; }
        public StockTransactionType TransactionType { get; set; }
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public string? Description { get; set; }

        public Product Product { get; set; } = null!;
        public Warehouse Warehouse { get; set; } = null!;
        public StorageLocation StorageLocation { get; set; } = null!;
    }
}
