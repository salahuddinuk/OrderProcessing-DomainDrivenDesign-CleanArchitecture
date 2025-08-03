using OrderProcessing.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        public string AddressValue { get; }
        public Address(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Address cannot be empty.");
            
            AddressValue = value;
        }
        private Address() { } // For EF Core, throws error otherwise

        protected override IEnumerable<object> GetEqualityComponents() => [AddressValue];
    }
}
