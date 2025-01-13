namespace OrderManagement.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderDetail> Items { get; set; }
        public decimal TotalPrice { get; set; }

        // Constructor fără parametri pentru EF Core
        public Order()
        {
            Items = new List<OrderDetail>();
        }

        // Constructor pentru inițializarea unei noi comenzi
        public Order(Guid userId, List<OrderDetail> items, decimal totalPrice)
        {
            Id = Guid.NewGuid();
            UserId = userId;
            OrderDate = DateTime.UtcNow;
            Items = items ?? new List<OrderDetail>();
            TotalPrice = totalPrice;
        }
    }
}
