using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories;
using OrderProcessing.Infrastructure.Data;

namespace OrderProcessing.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderProcessingDbContext _context;
        public ProductRepository(OrderProcessingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync(); // Retrieves all products from the database asynchronously
        }
        public async Task<Product> GetProductByIdAsync(Guid productId)
        {
            return await _context.Products.FindAsync(productId); // Used to retrieve an product by its Id
        }
        public async Task<int> AddStockAsync(Guid productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Stock += quantity; // Increase stock by the quantity added
                await _context.SaveChangesAsync();
                return product.Stock; // Return the updated stock count
            }
            return -1; // Return -1 if product not found
        }
        public async Task<int> DecreaseStockAsync(Guid productId, int quantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Stock -= quantity; // Decrease stock by the quantity ordered
                await _context.SaveChangesAsync();
            }
            return -1; // Return -1 if product not found
        }
        public async Task<int> GetStockAsync(Guid productId)
        {
            var product = await _context.Products.FindAsync(productId);
            return product?.Stock ?? 0; // return 0 if product not found or stock is null
        }
        public async Task<Guid> AddProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));
            await _context.Products.AddAsync(product); // Add product to the context
            await _context.SaveChangesAsync();
            return product.ProductId; // Return the ID of the newly added product
        }
    }
}
