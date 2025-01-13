using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure;

namespace OrderManagement.Application.Services
{
    public class InvoiceService
    {
        private readonly DatabaseContext _context;

        public InvoiceService(DatabaseContext context)
        {
            _context = context;
        }

        // Crearea unei facturi
        public async Task<InvoiceResponseDto> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            // Verifică dacă există comanda specificată
            var order = await _context.Orders
                .Include(o => o.Items) // Include produsele din comandă
                .FirstOrDefaultAsync(o => o.Id == dto.OrderId);

            if (order == null)
                throw new InvalidOperationException("Order not found.");

            // Calculează suma totală a facturii
            var totalAmount = order.Items.Sum(i => i.TotalPrice);

            // Creează factura
            var invoice = new Invoice
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                IssueDate = DateTime.UtcNow,
                TotalAmount = totalAmount
            };

            // Adaugă factura în baza de date
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();

            // Returnează detalii despre factură
            return new InvoiceResponseDto
            {
                InvoiceId = invoice.Id,
                OrderId = order.Id,
                IssueDate = invoice.IssueDate,
                TotalAmount = invoice.TotalAmount,
                User = new UserDetailsDto
                {
                    Name = (await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId)).Name,
                    Email = (await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId)).Email
                },
                Items = order.Items.Select(i => new OrderItemDetailsDto
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == i.ProductId)?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }

        // Obținerea unei facturi pe baza ID-ului
        public async Task<InvoiceResponseDto> GetInvoiceAsync(Guid invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Order)
                .ThenInclude(o => o.Items)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                throw new InvalidOperationException("Invoice not found.");

            var order = invoice.Order;

            return new InvoiceResponseDto
            {
                InvoiceId = invoice.Id,
                OrderId = invoice.OrderId,
                IssueDate = invoice.IssueDate,
                TotalAmount = invoice.TotalAmount,
                User = new UserDetailsDto
                {
                    Name = (await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId)).Name,
                    Email = (await _context.Users.FirstOrDefaultAsync(u => u.Id == order.UserId)).Email
                },
                Items = order.Items.Select(i => new OrderItemDetailsDto
                {
                    ProductName = _context.Products.FirstOrDefault(p => p.Id == i.ProductId)?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            };
        }

        // Obținerea tuturor facturilor
        public async Task<List<InvoiceResponseDto>> GetAllInvoicesAsync()
        {
            var invoices = await _context.Invoices
                .Include(i => i.Order)
                .ThenInclude(o => o.Items)
                .ToListAsync();

            // Încarcă utilizatorii și produsele necesare într-o singură interogare
            var users = await _context.Users.ToListAsync();
            var products = await _context.Products.ToListAsync();

            return invoices.Select(invoice => new InvoiceResponseDto
            {
                InvoiceId = invoice.Id,
                OrderId = invoice.OrderId,
                IssueDate = invoice.IssueDate,
                TotalAmount = invoice.TotalAmount,
                User = new UserDetailsDto
                {
                    Name = users.FirstOrDefault(u => u.Id == invoice.Order.UserId)?.Name,
                    Email = users.FirstOrDefault(u => u.Id == invoice.Order.UserId)?.Email
                },
                Items = invoice.Order.Items.Select(i => new OrderItemDetailsDto
                {
                    ProductName = products.FirstOrDefault(p => p.Id == i.ProductId)?.Name,
                    Quantity = i.Quantity,
                    UnitPrice = i.UnitPrice,
                    TotalPrice = i.TotalPrice
                }).ToList()
            }).ToList();
        }

    }
}
