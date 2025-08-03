using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.ValueObjects;
using OrderProcessing.Infrastructure.Data;
using OrderProcessing.Infrastructure.Repositories;

namespace OrderProcessing.Tests.Infrastructure
{
    public class OrderRepositoryTests : IDisposable
    {
        private readonly OrderProcessingDbContext _context;
        private readonly OrderRepository _repository;

        public OrderRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<OrderProcessingDbContext>()              
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // From .NET 9 > UseInMemoryDatabase not part of Microsoft.EntityFrameworkCore namespace, Microsoft.EntityFrameworkCore.InMemory package needed.
                .Options;

            _context = new OrderProcessingDbContext(options);
            _repository = new OrderRepository(_context);

        }

        [Fact]
        public async Task AddAsync_Should_Persist_Order()
        {
            // Arrange
            var order = new Order(              
                new Address("Frankfurt"),
                new EmailAddress("example@email.com"),
                new CreditCard("4111111111111111"),
                 new List<OrderItem>
                {
                   new(new ProductDetail(Guid.NewGuid(), "Monitor", 200m), 1)
                }
            );

            // Act
            await _repository.AddOrderAsync(order);

            var dbOrder = await _context.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

            // Assert
            Assert.NotNull(dbOrder);
            Assert.Single(dbOrder.Items);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
