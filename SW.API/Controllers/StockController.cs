using Microsoft.AspNetCore.Mvc;
using SW.ENTITY.Dtos.Stock;
using SW.MANAGER.Interfaces;

namespace SW.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly IStockManager _stockManager;

    public StockController(IStockManager stockManager)
    {
        _stockManager = stockManager;
    }

    [HttpPost("in")]
    public async Task<IActionResult> StockIn([FromBody] StockInDto dto)
    {
        var result = await _stockManager.StockInAsync(dto);

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            403 => Forbid(),
            _ => Ok(result)
        };
    }

    [HttpPost("out")]
    public async Task<IActionResult> StockOut([FromBody] StockOutDto dto)
    {
        var result = await _stockManager.StockOutAsync(dto);

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            403 => Forbid(),
            _ => Ok(result)
        };
    }
}