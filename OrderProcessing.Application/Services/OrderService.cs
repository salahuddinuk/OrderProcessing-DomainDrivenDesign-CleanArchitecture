using FluentValidation;
using Microsoft.Extensions.Logging;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories;
using OrderProcessing.Domain.ValueObjects;
using OrderProcessing.Infrastructure.Data;
using Serilog;
using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IOrderService> _logger;
        private readonly IValidator<CreateOrderRequest> _validator;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, ILogger<IOrderService> logger, IValidator<CreateOrderRequest> validator)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Guid> CreateOrderAsync(CreateOrderRequest orderRequest)
        {
            try
            {
                _logger.LogInformation("Creating new order for email: {Email}", orderRequest.InvoiceEmailAddress);
                
                _logger.LogInformation("New order data: {orderRequest}", System.Text.Json.JsonSerializer.Serialize(orderRequest));

                // auto validation is enabled in program.cs
                //var validate = await _validator.ValidateAsync(orderRequest);
                //if (!validate.IsValid)
                //{
                //    _logger.LogWarning("Validation failed for CreateOrderRequest: {@errors}", validate.Errors);
                //    throw new ValidationException(validate.Errors);
                //}

                var items = new List<OrderItem>();


                foreach (var item in orderRequest.Items)
                {

                    ProductDetail productDetail = new ProductDetail
                    (
                        // taking from request
                        item.ProductId,
                        item.ProductName,
                        item.ProductPrice

                    // from db
                    //product.ProductId,
                    //product.Name,
                    //product.Price
                    );

                    if (items.Any(i => i.Product.Equals(productDetail)))
                    {
                        _logger.LogWarning("Duplicate product cannot be added to the order.");
                        throw new ArgumentException("Duplicate product cannot be added to the order.");                        
                    }

                    items.Add(new OrderItem(productDetail, item.Quantity));
                }


               

                var invoiceAddress = new Address(orderRequest.InvoiceAddress);
                var invoiceEmailAddress = new EmailAddress(orderRequest.InvoiceEmailAddress);
                var invoiceCreditCard = new CreditCard(orderRequest.InvoiceCreditCard);


                Order order = new Order(
                    invoiceAddress,
                    invoiceEmailAddress,
                    invoiceCreditCard,
                    items
                    );

                await _orderRepository.AddOrderAsync(order);
                _logger.LogInformation("New order created with id: {orderid}", order.OrderId);


                return order.OrderId; // Return the ID of the newly created order
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating order for email: {Email}", orderRequest.InvoiceEmailAddress);
                throw; // Re-throw the exception to be handled by the caller
            }
        }

        public async Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Fetching order with ID: {OrderId}", orderId);

                Order order = await _orderRepository.GetOrderByIdAsync(orderId)
                    ?? throw new ArgumentException($"Order with ID {orderId} not found.");

                OrderDto orderDto = new OrderDto();
                orderDto.OrderId = order.OrderId;
                orderDto.InvoiceAddress = order.InvoiceAddress.AddressValue;
                orderDto.InvoiceEmailAddress = order.InvoiceEmailAddress.EmailValue;
                orderDto.InvoiceCreditCard = order.InvoiceCreditCard.CardNumberValue;

                orderDto.Items = order.Items.Select(item => new OrderItemDto
                {
                    ProductId = item.Product.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                }).ToList();

                orderDto.TotalAmount = order.Items.Sum(item => item.Product.Price * item.Quantity);
                orderDto.CreatedAt = order.CreatedAt;

                return orderDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching order with ID: {OrderId}", orderId);
                throw; // Re-throw the exception to be handled by the caller
            }

        }
    }
}
