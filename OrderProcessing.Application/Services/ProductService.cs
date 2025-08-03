using FluentValidation;
using Microsoft.Extensions.Logging;
using OrderProcessing.Application.DTOs;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories;

namespace OrderProcessing.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<IProductService> _logger;
        private readonly IValidator<CreateProductRequest> _validator;

        public ProductService(IProductRepository productRepository, ILogger<IProductService> logger, IValidator<CreateProductRequest> validator)
        {
            _productRepository = productRepository;
            _logger = logger;
            _validator = validator;
        }

        public async Task<Guid> CreateProductAsync(CreateProductRequest productRequest)
        {
            try
            {
                if (productRequest == null)
                {
                    _logger.LogError("CreateProductAsync request is null.");
                    throw new ArgumentNullException(nameof(productRequest), "Product request cannot be null.");
                }

                _logger.LogInformation("Creating new order for product name: {name}", productRequest.Name);

                var validate = await _validator.ValidateAsync(productRequest);
                if (!validate.IsValid)
                {
                    _logger.LogWarning("Validation failed for CreateProductAsync: {@errors}", validate.Errors);
                    throw new ValidationException(validate.Errors);
                }

                Product product = new Product(
                    productRequest.Name,
                    productRequest.Price,
                    productRequest.Stock
                    );

                await _productRepository.AddProductAsync(product);

                return product.ProductId; // Return the ID of the newly created product
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a product.");
                throw;
            }
        }

        public async Task<int> AddProductStockAsync(Guid productId, int newStock)
        {
            try
            {
                if (newStock < 0)
                {
                    _logger.LogWarning("Attempted to set negative stock for product ID: {productId}", productId);
                    throw new ArgumentException("Stock cannot be negative.");
                }

                _logger.LogInformation("Updating product stock for product ID: {productId} + Stock: {newStock}", productId, newStock);
                Product product = await _productRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found for product Id: {productId}", productId);
                    throw new ArgumentException($"Product not found for product Id {productId} not found.");
                }
                if (newStock < 0)
                {
                    _logger.LogWarning("Attempted to set negative stock for product ID: {productId}", productId);
                    throw new ArgumentException("Stock cannot be negative.");
                }
                return await _productRepository.AddStockAsync(productId, newStock); // new total available stock
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding stock for product ID: {productId}", productId);
                throw;
            }
        }
        public async Task<int> DecreaseProductStockAsync(Guid productId, int orderQuantity)
        {
            try
            {
                if (orderQuantity < 0)
                {
                    _logger.LogWarning("Attempted to decrease stock by a negative quantity for product ID: {productId}", productId);
                    throw new ArgumentException("Order quantity cannot be negative.");
                }

                _logger.LogInformation("Decreasing product stock for product ID: {productId} - order quantity: {newStock}", productId, orderQuantity);
                Product product = await _productRepository.GetProductByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found for product Id: {productId}", productId);
                    throw new ArgumentException($"Product not found for product Id {productId} not found.");
                }
                if (orderQuantity < 0)
                {
                    _logger.LogWarning("Attempted to set negative stock for product ID: {productId}", productId);
                    throw new ArgumentException("Stock cannot be negative.");
                }
                return await _productRepository.DecreaseStockAsync(productId, orderQuantity); // new total available stock
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while decreasing stock for product ID: {productId}", productId);
                throw;
            }
        }

    }
}
