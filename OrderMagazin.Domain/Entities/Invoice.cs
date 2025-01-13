namespace OrderManagement.Domain.Entities
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Relație cu entitatea `Order`
        public Order Order { get; set; }

        public Invoice() { }

        public Invoice(Guid orderId, decimal totalAmount)
        {
            Id = Guid.NewGuid();
            OrderId = orderId;
            IssueDate = DateTime.UtcNow;
            TotalAmount = totalAmount;
        }
    }
}
