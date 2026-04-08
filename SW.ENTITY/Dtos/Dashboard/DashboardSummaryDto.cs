using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Dtos.Dashboard
{
    public class DashboardSummaryDto
    {
        public int TotalProductCount { get; set; }
        public int TotalStockCount { get; set; }
        public int CriticalStockProductCount { get; set; }
        public int TodayStockInCount { get; set; }
        public int TodayStockOutCount { get; set; }
        public int TotalWarehouseCount { get; set; }
        public int TotalLocationCount { get; set; }
    }
}
