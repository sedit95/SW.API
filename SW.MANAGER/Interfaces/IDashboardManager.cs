using SW.ENTITY.Dtos.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.MANAGER.Interfaces
{
    public interface IDashboardManager
    {
        Task<DashboardSummaryDto> GetSummaryAsync(string companyId);
    }
}
