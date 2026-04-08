using Microsoft.AspNetCore.Mvc;
using SW.ENTITY.Dtos.Product;
using SW.MANAGER.Interfaces;

namespace SW.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductManager _productManager;

    public ProductController(IProductManager productManager)
    {
        _productManager = productManager;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id, [FromQuery] string companyId)
    {
        if (string.IsNullOrWhiteSpace(companyId))
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur."
            });
        }

        var result = await _productManager.GetByIdAsync(id, companyId);

        if (result == null)
        {
            return NotFound(new
            {
                IsSuccess = false,
                Message = "Ürün bulunamadı."
            });
        }

        if (result.CompanyId != companyId)
        {
            return Forbid();
        }

        return Ok(new
        {
            IsSuccess = true,
            Message = "Ürün başarıyla getirildi.",
            Data = result
        });
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetPaged([FromQuery] ProductPagedRequestDto request)
    {
        if (string.IsNullOrWhiteSpace(request.CompanyId))
        {
            return BadRequest(new
            {
                IsSuccess = false,
                Message = "CompanyId alanı zorunludur."
            });
        }

        var result = await _productManager.GetPagedAsync(request);

        if (!result.IsSuccess)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto dto)
    {
        var result = await _productManager.CreateAsync(dto);

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            403 => Forbid(),
            _ => Ok(result)
        };
    }

    [HttpPost("update")]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto dto)
    {
        var result = await _productManager.UpdateAsync(dto);

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            403 => Forbid(),
            _ => Ok(result)
        };
    }

    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody] ProductDeleteDto dto)
    {
        var result = await _productManager.DeleteAsync(dto);

        return result.StatusCode switch
        {
            400 => BadRequest(result),
            404 => NotFound(result),
            403 => Forbid(),
            _ => Ok(result)
        };
    }
}