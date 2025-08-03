using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderProcessing.Domain.Entities;
using OrderProcessing.Infrastructure.Data;

namespace OrderProcessing.Tests.Integration
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //// Remove the existing DbContext registration
                //var descriptor = services.SingleOrDefault(
                //    d => d.ServiceType == typeof(DbContextOptions<OrderProcessingDbContext>));
                //if (descriptor != null)
                //{
                //    services.Remove(descriptor);
                //}

                //// Add a test-specific in-memory database
                //services.AddDbContext<OrderProcessingDbContext>(options =>
                //{
                //    options.UseInMemoryDatabase("TestDb");
                //});

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Ensure DB is created and seeded
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<OrderProcessingDbContext>();
                    db.Database.EnsureCreated();

                    // Optionally: Seed test data here
                    SeedTestData(db);
                }
            });
        }

        private void SeedTestData(OrderProcessingDbContext db)
        {
            var product = new Product
            (
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                "Gaming Laptop",
                1499.99m,
                10
            );

            db.Products.Add(product);
            db.SaveChanges();
        }
    }
}
