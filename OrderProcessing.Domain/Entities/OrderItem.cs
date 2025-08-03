using OrderProcessing.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.Entities
{
    public class OrderItem
    {
        [Key]
        public Guid OrderItemId { get; set; }
        public ProductDetail Product { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => Product.Price * Quantity;

        private OrderItem() { } // For EF Core
        public OrderItem(ProductDetail product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "Product cannot be null.");
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            Product = product;
            Quantity = quantity;
        }
    }
}
