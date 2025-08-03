using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using OrderProcessing.Application.DTOs;
using System.Net;
using System.Net.Http.Json;

namespace OrderProcessing.Tests.Integration
{
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public OrdersControllerTests(WebApplicationFactory<Program> factory)
        {
            factory = factory.WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
            });
            _client = factory.CreateClient();            
        }

        [Fact]
        public async Task CreateOrder_Should_Return_Created_OrderId()
        {
            // Arrange
            var request = new CreateOrderRequest
            {
                InvoiceAddress = "Main Street 1, 60311 Frankfurt",
                InvoiceEmailAddress = "test@example.com",
                InvoiceCreditCard = "4111111111111111",
                Items = new List<CreateOrderItemDto>
            {
                new CreateOrderItemDto
                {
                    ProductId = Guid.NewGuid(), //Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    ProductName = "Gaming Laptop",
                    Quantity = 1,
                    ProductPrice = 1499.99m
                }
            }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/orders", request);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
            var responseBody = await response.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            responseBody.Should().ContainKey("orderId");
        }

        [Fact]
        public async Task GetOrderById_Should_Return_Valid_Order()
        {
            // Arrange: Create an order first
            var createRequest = new CreateOrderRequest
            {
                InvoiceAddress = "Test Address, Frankfurt",
                InvoiceEmailAddress = "user@example.com",
                InvoiceCreditCard = "4111111111111111",
                Items = new List<CreateOrderItemDto>
                {
                    new CreateOrderItemDto
                    {
                        ProductId = Guid.NewGuid(), //Guid.Parse("11111111-1111-1111-1111-111111111111"),
                        ProductName = "Gaming Laptop",
                        Quantity = 1,
                        ProductPrice = 1499.99m
                    }
                }
            };

            var createResponse = await _client.PostAsJsonAsync("/api/orders", createRequest);
            createResponse.EnsureSuccessStatusCode();
            var created = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, Guid>>();
            var orderId = created["orderId"];

            // Act
            var getResponse = await _client.GetAsync($"/api/orders/{orderId}");

            // Assert
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var order = await getResponse.Content.ReadAsStringAsync();
            order.Should().Contain("Gaming Laptop");
            order.Should().Contain("user@example.com");
        }
    }
}
