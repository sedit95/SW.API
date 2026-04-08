using SW.ENTITY.Dtos.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Dtos.Product
{
    public class ProductPagedRequestDto : PagedRequestDto
    {
        public int? ProductCategoryId { get; set; }
        public int? WarehouseId { get; set; }
        public bool? IsCriticalStock { get; set; }
    }
}
