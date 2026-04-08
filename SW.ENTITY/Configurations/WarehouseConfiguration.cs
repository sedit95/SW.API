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
    public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
    {
        public void Configure(EntityTypeBuilder<Warehouse> builder)
        {
            builder.ToTable("Warehouses");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.WarehouseCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.WarehouseName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(x => x.Description)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            builder.Property(x => x.IsDeleted)
                .IsRequired();

            builder.HasIndex(x => x.CompanyId);
            builder.HasIndex(x => x.IsDeleted);
            builder.HasIndex(x => new { x.CompanyId, x.WarehouseCode })
                .IsUnique();

            builder.HasMany(x => x.StorageLocations)
                .WithOne(x => x.Warehouse)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.StockTransactions)
                .WithOne(x => x.Warehouse)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
