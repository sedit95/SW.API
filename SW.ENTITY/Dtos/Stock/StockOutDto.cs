using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Dtos.Stock
{
    public class StockOutDto
    {
        public string? CompanyId { get; set; }
        public int ProductId { get; set; }
        public int WarehouseId { get; set; }
        public int StorageLocationId { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
    }
}
