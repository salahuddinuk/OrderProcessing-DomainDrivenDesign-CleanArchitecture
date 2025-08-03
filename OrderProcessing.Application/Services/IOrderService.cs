using OrderProcessing.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Application.Services
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(CreateOrderRequest request);
        Task<OrderDto?> GetOrderByIdAsync(Guid orderId);
    }
}
