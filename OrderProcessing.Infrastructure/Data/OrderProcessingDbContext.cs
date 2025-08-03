using Microsoft.EntityFrameworkCore;
using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Infrastructure.Data
{
    public class OrderProcessingDbContext : DbContext
    {
        public DbSet<Order> Orders => Set<Order>();

        public DbSet<Product> Products => Set<Product>();
        public OrderProcessingDbContext(DbContextOptions<OrderProcessingDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderProcessingDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
