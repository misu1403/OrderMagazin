namespace OrderManagement.Application.DTOs
{
    public class CreateOrderDto
    {
        public Guid UserId { get; set; }
        public List<CreateOrderItemDto> Items { get; set; }
    }

    public class CreateOrderItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
