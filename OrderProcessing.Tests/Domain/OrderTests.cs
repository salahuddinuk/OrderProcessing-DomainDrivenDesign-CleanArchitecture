using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Tests.Domain
{
    public class OrderTests
    {
        [Fact]
        public void Order_Should_Calculate_Total_Price_Correctly()
        {
            // Arrange
            var items = new List<OrderItem>
        {
            new(new ProductDetail(Guid.NewGuid(), "Laptop", 1200m), 1),
            new(new ProductDetail(Guid.NewGuid(), "Mouse", 25m), 2)
        };
            var address = new Address("Teststraße 1, 60311 Frankfurt");
            var email = new EmailAddress("test@example.com");
            var card = new CreditCard("4111111111111111");

            // Act
            var order = new Order(address, email, card, items);
            var total = order.TotalAmount;

            // Assert
            Assert.Equal(1250m, total);
        }

        [Fact]
        public void OrderItem_Should_Throw_If_Quantity_Is_Zero()
        {
            Assert.Throws<ArgumentException>(() =>
                new OrderItem(new ProductDetail(Guid.NewGuid(), "Laptop", 1200m), 0));
        }
    }
}