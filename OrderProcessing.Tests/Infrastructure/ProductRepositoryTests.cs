using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.ValueObjects;
using OrderProcessing.Infrastructure.Data;
using OrderProcessing.Infrastructure.Repositories;

namespace OrderProcessing.Tests.Infrastructure
{
    public class ProductRepositoryTests : IDisposable
    {
        private readonly OrderProcessingDbContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<OrderProcessingDbContext>()              
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) // From .NET 9 > UseInMemoryDatabase not part of Microsoft.EntityFrameworkCore namespace, Microsoft.EntityFrameworkCore.InMemory package needed.
                .Options;

            _context = new OrderProcessingDbContext(options);
            _repository = new ProductRepository(_context);

        }

        [Fact]
        public async Task AddAsync_Should_Persist_Product()
        {
            // Arrange
            var product = new Product("Monitor", 200m, 20);            

            // Act
            await _repository.AddProductAsync(product);

            var dbOrder = await _context.Products.FirstOrDefaultAsync(o => o.ProductId == product.ProductId);

            // Assert
            Assert.NotNull(dbOrder);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
