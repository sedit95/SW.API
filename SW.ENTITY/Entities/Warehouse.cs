using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Entities
{
    public class Warehouse : BaseEntity
    {
        public string? WarehouseCode { get; set; }
        public string? WarehouseName { get; set; }
        public string? Description { get; set; }

        public ICollection<StorageLocation> StorageLocations { get; set; } = new List<StorageLocation>();
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
