using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure;

namespace OrderManagement.Application.Services
{
    public class OrderService
    {
        private readonly DatabaseContext _context;

        public OrderService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> CreateOrderAsync(CreateOrderDto dto)
        {
            // Calculează suma totală
            decimal totalPrice = dto.Items.Sum(i => i.Quantity * i.UnitPrice);

            // Creează comanda principală
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = dto.UserId,
                OrderDate = DateTime.UtcNow,
                TotalPrice = totalPrice, // Setează suma totală
                Items = dto.Items.Select(i => new OrderDetail
                {
                    Id = Guid.NewGuid(),
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice
                }).ToList()
            };

            // Adaugă comanda în baza de date
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Returnează răspunsul
            return new OrderResponseDto
            {
                OrderId = order.Id,
                TotalPrice = order.TotalPrice
            };
        }

        public async Task<OrderDetailsDto> GetOrderDetailsAsync(Guid orderId)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null)
                throw new InvalidOperationException("Order not found.");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            return new OrderDetailsDto
            {
                OrderId = order.Id,
                User = new UserDetailsDto
                {
                    Name = user.Name,
                    Email = user.Email
                },
                Items = order.Items.Select(i => new OrderItemDetailsDto
                {
                    ProductName = _context.Products.First(p => p.Id == i.ProductId).Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList(),
                TotalPrice = order.TotalPrice
            };
        }

        public async Task<List<OrderDetailsDto>> GetAllOrdersAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();

            return orders.Select(order => new OrderDetailsDto
            {
                OrderId = order.Id,
                User = new UserDetailsDto
                {
                    Name = _context.Users.First(u => u.Id == order.UserId).Name,
                    Email = _context.Users.First(u => u.Id == order.UserId).Email
                },
                Items = order.Items.Select(item => new OrderItemDetailsDto
                {
                    ProductName = _context.Products.First(p => p.Id == item.ProductId).Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList(),
                TotalPrice = order.TotalPrice
            }).ToList();
        }

    }
}
