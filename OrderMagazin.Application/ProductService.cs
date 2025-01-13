using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application
{
    public class ProductService
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Guid> AddProductAsync(AddProductDto dto)
        {
            var product = new Product(dto.Name, dto.Description, dto.Price, dto.Stock);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _context.Products.FindAsync(id);
        }
    }

}
