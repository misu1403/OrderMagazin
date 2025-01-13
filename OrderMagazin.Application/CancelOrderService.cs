using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Infrastructure;

namespace OrderManagement.Application.Services
{
    public class CancelOrderService
    {
        private readonly DatabaseContext _context;

        public CancelOrderService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task CancelOrderAsync(Guid orderId)
        {
            var order = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
                throw new InvalidOperationException("Order not found.");

            // Șterge facturile asociate
            var invoices = _context.Invoices.Where(i => i.OrderId == orderId);
            _context.Invoices.RemoveRange(invoices);

            // Șterge detaliile comenzii
            _context.OrderDetail.RemoveRange(order.Items);

            // Șterge comanda
            _context.Orders.Remove(order);

            await _context.SaveChangesAsync();
        }


    }
}
