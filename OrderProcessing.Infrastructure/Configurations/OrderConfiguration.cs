using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderProcessing.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderProcessing.Infrastructure.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(o => o.OrderId);

            // Value Object: Address
            builder.OwnsOne(o => o.InvoiceAddress, a =>
            {
                a.Property(p => p.AddressValue)
                 .HasColumnName("InvoiceAddress")
                 .HasMaxLength(200)
                 .IsRequired();
            });

            // Value Object: Email
            builder.OwnsOne(o => o.InvoiceEmailAddress, e =>
            {
                e.Property(p => p.EmailValue)
                 .HasColumnName("InvoiceEmail")
                 .HasMaxLength(150)
                 .IsRequired();
            });

            // Value Object: Credit Card
            builder.OwnsOne(o => o.InvoiceCreditCard, c =>
            {
                c.Property(p => p.CardNumberValue)
                 .HasColumnName("InvoiceCreditCard")
                 .HasMaxLength(16)
                 .IsRequired();
            });

            // Owned collection of ProductItem
            builder.OwnsMany(o => o.Items, item =>
            {
                item.ToTable("OrderItems");

                item.WithOwner().HasForeignKey("OrderId");

                // 👇 Mapping Product as Value Object
                item.OwnsOne(i => i.Product, p =>
                {
                    p.Property(pp => pp.ProductId).HasColumnName("ProductId");
                    p.Property(pp => pp.Name).HasColumnName("Name").HasMaxLength(100);
                    p.Property(pp => pp.Price).HasColumnName("Price").HasColumnType("decimal(18,2)");
                });

                item.Property(i => i.Quantity).IsRequired();

                
            });

            //// Owned Collection: Product Items
            //builder.OwnsMany(o => o.items, item =>
            //{
            //    item.ToTable("order_items");

            //    item.WithOwner().HasForeignKey("order_id");
            //    item.HasKey("order_id", "ProductId"); // Composite Key

            //    item.Property(p => p.Product.ProductId).HasColumnName("product_id").HasMaxLength(50);
            //    item.Property(p => p.Product.Name).HasColumnName("product_name").HasMaxLength(100);
            //    item.Property(p => p.Product.Price).HasColumnName("product_price");
            //    //item.Property(p => p.ProductPrice).HasColumnName("product_price").HasColumnType("decimal(18,2)");
            //});
        }
    }
}
