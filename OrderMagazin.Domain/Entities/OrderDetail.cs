using OrderMagazin.Domain.Entities;

namespace OrderManagement.Domain.Entities
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;

        public Order Order { get; set; } // Relație cu Order
        public Product Product { get; set; } // Relație cu Product
    }
}
