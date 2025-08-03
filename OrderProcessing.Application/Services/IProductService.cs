using OrderProcessing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Services
{
    public interface IProductService
    {
        Task<Guid> CreateProductAsync(CreateProductRequest request);
        Task<int> AddProductStockAsync(Guid productId, int newStock);
        Task<int> DecreaseProductStockAsync(Guid productId, int newStock);
    }
}
