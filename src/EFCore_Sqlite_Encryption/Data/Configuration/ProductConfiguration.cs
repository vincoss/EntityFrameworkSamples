using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;


namespace EFCore_Sqlite_Encryption.Data.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product))
                   .HasIndex(x => new { x.ProductId, x.ProductName }).IsUnique();

            builder.ToTable(nameof(Product))
                   .HasIndex(x => x.ProductName).IsUnique();

            builder.HasKey(x => x.ProductId);

            builder.Property(t => t.ProductId)
                 .IsRequired()
                 .HasColumnType("INTEGER");

            builder.Property(t => t.ProductName)
                   .IsRequired()
                   .HasColumnType("NVARCHAR(256) COLLATE NOCASE");

            builder.Property(x => x.CreatedDateTimeUtc)
                   .IsRequired()
                   .HasDefaultValue(DateTime.UtcNow)
                   .HasColumnType("DATETIME");
        }
    }
}
