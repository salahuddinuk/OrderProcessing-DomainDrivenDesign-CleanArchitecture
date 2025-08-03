using OrderProcessing.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        public string EmailValue { get; }

        public EmailAddress(string email) {
            if (!Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("Invalid email address.");
            EmailValue = email;
        }

        private EmailAddress() { } // For EF Core, throws error otherwise

        protected override IEnumerable<object> GetEqualityComponents() => [EmailValue];
    }
}
