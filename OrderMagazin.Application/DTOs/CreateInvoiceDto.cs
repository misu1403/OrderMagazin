using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs
{
    public class CreateInvoiceDto
    {
        public Guid OrderId { get; set; }
    }

    public class InvoiceResponseDto
    {
        public Guid InvoiceId { get; set; }
        public Guid OrderId { get; set; }
        public DateTime IssueDate { get; set; }
        public decimal TotalAmount { get; set; }
        public UserDetailsDto User { get; set; }
        public List<OrderItemDetailsDto> Items { get; set; }
    }

}

