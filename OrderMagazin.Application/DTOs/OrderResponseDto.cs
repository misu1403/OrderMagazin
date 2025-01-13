using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
    }
}

