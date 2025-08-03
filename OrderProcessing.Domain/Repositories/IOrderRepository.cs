using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Domain.Repositories
{
    public interface IOrderRepository
    {
        Task<List<Entities.Order>> GetAllOrdersAsync();
        Task<Entities.Order> GetOrderByIdAsync(Guid orderId);
        Task AddOrderAsync(Entities.Order order);  
    }
}
