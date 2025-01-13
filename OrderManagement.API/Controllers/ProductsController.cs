using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(AddProductDto dto)
        {
            var productId = await _productService.AddProductAsync(dto);
            return CreatedAtAction(nameof(GetProduct), new { id = productId }, null);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
    }

}
