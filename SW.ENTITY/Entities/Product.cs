using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Entities
{
    public class Product : BaseEntity
    {
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public int ProductCategoryId { get; set; }
        public string? UnitType { get; set; }
        public int MinimumStockLevel { get; set; }
        public int CurrentStock { get; set; }

        public ProductCategory ProductCategory { get; set; } = null!;
        public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
    }
}
