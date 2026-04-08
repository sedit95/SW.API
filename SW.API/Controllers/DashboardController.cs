using Microsoft.AspNetCore.Mvc;
using SW.MANAGER.Interfaces;

namespace SW.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DashboardController : ControllerBase
{
    private readonly IDashboardManager _dashboardManager;

    public DashboardController(IDashboardManager dashboardManager)
    {
        _dashboardManager = dashboardManager;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur."
            });
        }

        var result = await _dashboardManager.GetSummaryAsync(companyId);

        return Ok(new
        {
            IsSuccess = true,
            Message = "Özet bilgiler başarıyla getirildi.",
            Data = result
        });
    }
}