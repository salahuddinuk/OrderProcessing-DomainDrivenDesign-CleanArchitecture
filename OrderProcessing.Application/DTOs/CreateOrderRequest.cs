using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.DTOs
{
    public class CreateOrderRequest
    {
        public List<CreateOrderItemDto> Items { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceEmailAddress { get; set; }
        public string InvoiceCreditCard { get; set; }
    }
}
