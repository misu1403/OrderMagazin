using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
    {
        // Creează comanda și primește ID-ul comenzii
        var orderResponse = await _orderService.CreateOrderAsync(dto);

        // Returnează detaliile comenzii create
        return CreatedAtAction(nameof(GetOrder), new { id = orderResponse.OrderId }, orderResponse);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        try
        {
            var orderDetails = await _orderService.GetOrderDetailsAsync(id);
            return Ok(orderDetails);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }


    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        try
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return Ok(orders);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = "An error occurred while retrieving orders.", Error = ex.Message });
        }
    }

}
