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
    public class StorageLocationConfiguration : IEntityTypeConfiguration<StorageLocation>
    {
        public void Configure(EntityTypeBuilder<StorageLocation> builder)
        {
            builder.ToTable("StorageLocations");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CompanyId)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LocationCode)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.LocationName)
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
            builder.HasIndex(x => new { x.CompanyId, x.WarehouseId, x.LocationCode })
                .IsUnique();

            builder.HasOne(x => x.Warehouse)
                .WithMany(x => x.StorageLocations)
                .HasForeignKey(x => x.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.StockTransactions)
                .WithOne(x => x.StorageLocation)
                .HasForeignKey(x => x.StorageLocationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
