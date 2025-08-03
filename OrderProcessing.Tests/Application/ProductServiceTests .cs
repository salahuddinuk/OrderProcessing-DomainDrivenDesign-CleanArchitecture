using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Application.Services;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories;

namespace OrderProcessing.Tests.Application
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IValidator<CreateProductRequest>> _productValidator = new();
        private readonly Mock<ILogger<IProductService>> _productLogger = new();

        private readonly IProductService _productService;

        public ProductServiceTests()
        {
            _productValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateProductRequest>(), default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _productService = new ProductService(_productRepo.Object, _productLogger.Object, _productValidator.Object);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Save_Product_And_Return_Id()
        {
            CreateOrderRequest createOrderRequest = new CreateOrderRequest();

            // Arrange
            // create a product to use in the order
            var product = new CreateProductRequest
            {
                Name = "Laptop",
                Price = 1799.99m,
                Stock = 10
            };

            // Act           
            var id = await _productService.CreateProductAsync(product);

            // Assert
            _productRepo.Verify(r => r.AddProductAsync(It.IsAny<Product>()), Times.Once);
            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public async Task CreateProductAsync_Should_Throw_ValidationException_On_Invalid_Request()
        {
            _productValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateProductRequest>(), default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("field", "error") }));

            var invalidRequest = new CreateProductRequest();

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _productService.CreateProductAsync(invalidRequest));
        }
    }
}