using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Dtos.Product
{
    public class ProductUpdateDto
    {
        public int Id { get; set; }
        public string? CompanyId { get; set; }
        public string? ProductCode { get; set; }
        public string? ProductName { get; set; }
        public int ProductCategoryId { get; set; }
        public string? UnitType { get; set; }
        public int MinimumStockLevel { get; set; }
    }
}
