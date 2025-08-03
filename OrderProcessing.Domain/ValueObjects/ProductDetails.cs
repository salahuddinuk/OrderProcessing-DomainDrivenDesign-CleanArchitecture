using OrderProcessing.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.ValueObjects
{
    public class ProductDetail : ValueObject
    {
        public Guid ProductId { get; }
        public string Name { get; }
        public decimal Price { get; }

        public ProductDetail(Guid productId, string name, decimal price)
        {
            ProductId = productId;
            Name = name;
            Price = price;
        }

        private ProductDetail() { } // For EF Core, throws error otherwise

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ProductId;
            yield return Name;
            yield return Price;
        }
    }
}
