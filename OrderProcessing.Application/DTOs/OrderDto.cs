using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OrderProcessing.Application.DTOs
{
    public class OrderDto
    {
        public Guid OrderId { get; set; }
        public string InvoiceAddress { get; set; }
        public string InvoiceEmailAddress { get; set; }
        [JsonIgnore]
        public string InvoiceCreditCard { get; set; } 
        public List<OrderItemDto> Items { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
