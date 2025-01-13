using System;
using System.Collections.Generic;

namespace OrderManagement.Application.DTOs
{
    public class OrderDetailsDto
    {
        public Guid OrderId { get; set; }
        public UserDetailsDto User { get; set; }
        public List<OrderItemDetailsDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }

    public class UserDetailsDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }

    public class OrderItemDetailsDto
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
