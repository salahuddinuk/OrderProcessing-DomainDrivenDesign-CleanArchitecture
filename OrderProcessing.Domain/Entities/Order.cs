using OrderProcessing.Domain.Base;
using OrderProcessing.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.Entities
{
    public class Order 
    {
        [Key]
        public Guid OrderId { get; set; }
        public Address InvoiceAddress { get; set; }
        public EmailAddress InvoiceEmailAddress { get; set; }
        public CreditCard InvoiceCreditCard { get; set; }
        
        public List<OrderItem> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(item => item.Subtotal);
        public DateTime CreatedAt { get; set; }

        private Order() { } // For EF Core
        public Order(Address address, EmailAddress emailAddress, CreditCard creditCard, IEnumerable<OrderItem> orderItems)
        {
            OrderId = Guid.NewGuid();
            InvoiceAddress = address ?? throw new ArgumentNullException(nameof(address), "Address cannot be null.");
            InvoiceEmailAddress = emailAddress ?? throw new ArgumentNullException(nameof(emailAddress), "Email address cannot be null.");
            InvoiceCreditCard = creditCard ?? throw new ArgumentNullException(nameof(creditCard), "Credit card cannot be null.");
            //Items = orderItems;

            Items.AddRange(orderItems ?? throw new ArgumentNullException(nameof(orderItems), "Order items cannot be null."));
            CreatedAt = DateTime.UtcNow;
        }
    }
}
