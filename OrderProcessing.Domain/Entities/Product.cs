using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.Entities
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Product(string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");
            if (stock < 0)
                throw new ArgumentException("Stock cannot be negative.");
            ProductId =Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
        }
        public Product(Guid id, string name, decimal price, int stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty.");
            if (price <= 0)
                throw new ArgumentException("Price must be greater than zero.");
            if (stock < 0)
                throw new ArgumentException("Stock cannot be negative.");

            ProductId = id;
            ProductId = Guid.NewGuid();
            Name = name;
            Price = price;
            Stock = stock;
        }
    }
}
