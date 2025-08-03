using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Domain.Repositories;
using OrderProcessing.Infrastructure.Data;

namespace OrderProcessing.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderProcessingDbContext _context;
        public OrderRepository(OrderProcessingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders.ToListAsync(); // Retrieves all orders from the database asynchronously
        }
        public async Task<Order> GetOrderByIdAsync(Guid orderId)
        {
            return await _context.Orders.FindAsync(orderId); // Used to retrieve an order by its ID
        }
        public async Task AddOrderAsync(Order order)
        {
            if (order == null) throw new ArgumentNullException(nameof(order));
            await _context.Orders.AddAsync(order); // Add the order to the context
            await _context.SaveChangesAsync();
        }
    }
}
