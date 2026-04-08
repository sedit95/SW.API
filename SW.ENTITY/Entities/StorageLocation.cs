using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Entities
{
    public class StorageLocation : BaseEntity
    {
        public int WarehouseId { get; set; }
        public string? LocationCode { get; set; }
        public string? LocationName { get; set; }
        public string? Description { get; set; }

        public Warehouse Warehouse { get; set; } = null!;
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
