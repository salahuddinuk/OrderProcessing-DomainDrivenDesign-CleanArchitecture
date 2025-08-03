using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid orderId);
        Task<int> AddStockAsync(Guid productId, int quantity);
        Task<int> DecreaseStockAsync(Guid productId, int quantity);
        Task<int> GetStockAsync(Guid productId);
        Task<Guid> AddProductAsync(Product product);
    }
}
