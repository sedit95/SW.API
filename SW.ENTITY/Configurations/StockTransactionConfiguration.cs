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
    public class StockTransactionConfiguration : IEntityTypeConfiguration<StockTransaction>
    {
        public void Configure(EntityTypeBuilder<StockTransaction> builder)
        {
            builder.ToTable("StockTransactions");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.TransactionType)
                .IsRequired();

            builder.Property(x => x.Quantity)
                .IsRequired();

            builder.Property(x => x.TransactionDate)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => x.TransactionDate);
            builder.HasIndex(x => new { x.CompanyId, x.ProductId });

            builder.HasOne(x => x.Product)
                .WithMany(x => x.StockTransactions)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Warehouse)
                .WithMany(x => x.StockTransactions)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.StorageLocation)
                .WithMany(x => x.StockTransactions)
                .HasForeignKey(x => x.StorageLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
