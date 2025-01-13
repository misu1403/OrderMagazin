using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class CancelOrderController : ControllerBase
{
    private readonly CancelOrderService _cancelOrderService;

    public CancelOrderController(CancelOrderService cancelOrderService)
    {
        _cancelOrderService = cancelOrderService;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelOrder(Guid id)
    {
        try
        {
            await _cancelOrderService.CancelOrderAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
