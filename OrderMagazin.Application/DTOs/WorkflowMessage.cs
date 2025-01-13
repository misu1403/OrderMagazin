using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.DTOs
{
    public class WorkflowMessage
    {
        public string WorkflowType { get; set; }
        public string Payload { get; set; }
    }
}

