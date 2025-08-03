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
    public class OrderServiceTests
    {
        private readonly Mock<IOrderRepository> _orderRepo = new();
        private readonly Mock<IProductRepository> _productRepo = new();
        private readonly Mock<IValidator<CreateOrderRequest>> _orderValidator = new();
        private readonly Mock<IValidator<CreateProductRequest>> _productValidator = new();
        private readonly Mock<ILogger<IOrderService>> _orderLogger = new();
        private readonly Mock<ILogger<IProductService>> _productLogger = new();

        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public OrderServiceTests()
        {
            _orderValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateOrderRequest>(), default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _productValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateProductRequest>(), default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult());

            _orderService = new OrderService(_orderRepo.Object, _productRepo.Object, _orderLogger.Object, _orderValidator.Object);
            _productService = new ProductService(_productRepo.Object, _productLogger.Object, _productValidator.Object);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Save_Order_And_Return_Id()
        {
            CreateOrderRequest createOrderRequest = new CreateOrderRequest();

            // Arrange
            // create a product to use in the order
            var testProductId = await _productService.CreateProductAsync(new CreateProductRequest
            {
                Name = "Phone",
                Price = 549.99m,
                Stock = 10
            });

            var request = new CreateOrderRequest
            {
                Items = new()
                {
                    new CreateOrderItemDto { ProductId = testProductId, ProductName = "Phone", Quantity = 1, ProductPrice = 549.99m }
                },
                InvoiceAddress = "Frankfurt",
                InvoiceEmailAddress = "user@example.com",
                InvoiceCreditCard = "4111111111111111"                
            };

            // Act           
            var id = await _orderService.CreateOrderAsync(request);

            // Assert
            _orderRepo.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            Assert.NotEqual(Guid.Empty, id);
        }

        [Fact]
        public async Task CreateOrderAsync_Should_Throw_ValidationException_On_Invalid_Request()
        {
            _orderValidator.Setup(v => v.ValidateAsync(It.IsAny<CreateOrderRequest>(), default))
                      .ReturnsAsync(new FluentValidation.Results.ValidationResult(new[] { new ValidationFailure("field", "error") }));

            var invalidRequest = new CreateOrderRequest();

            await Assert.ThrowsAsync<FluentValidation.ValidationException>(() =>
                _orderService.CreateOrderAsync(invalidRequest));
        }
    }
}