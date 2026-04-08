using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SW.ENTITY.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SW.ENTITY.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ProductCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.ProductName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.UnitType)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.MinimumStockLevel)
                .IsRequired();

            builder.Property(x => x.CurrentStock)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => new { x.CompanyId, x.ProductCode })
                .IsUnique();

            builder.HasOne(x => x.ProductCategory)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.StockTransactions)
                .WithOne(x => x.Product)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
