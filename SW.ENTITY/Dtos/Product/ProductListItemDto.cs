using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Dtos.Product
{
    public class ProductListItemDto
    {
        public int Id { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? UnitType { get; set; }
        public int MinimumStockLevel { get; set; }
        public int CurrentStock { get; set; }
    }
}
