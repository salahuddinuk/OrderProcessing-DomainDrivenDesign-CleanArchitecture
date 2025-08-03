using OrderProcessing.Domain.Base;
using System.Text.RegularExpressions;

namespace OrderProcessing.Domain.ValueObjects
{
    public class CreditCard : ValueObject
    {
        public string CardNumberValue { get; }

        public CreditCard(string cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || cardNumber.Length < 16)
                throw new ArgumentException("Invalid credit card number.");
            
            CardNumberValue = cardNumber;
        }

        private CreditCard() { } // For EF Core, throws error otherwise

        protected override IEnumerable<object> GetEqualityComponents() => [CardNumberValue];

    }
}
