using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Contexto.Configuraciones
{
    public class ProductConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {

            builder.HasKey(p => p.Id);

            builder.Property(p => p.SKU)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.HasIndex(p => p.SKU)
                   .IsUnique();

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(p => p.Category)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.CreatedBy)
                   .HasMaxLength(100);

            builder.Property(p => p.UpdatedBy)
                   .HasMaxLength(100);

            builder.Property(p => p.UnitPrice)
                   .HasColumnType("decimal(18,2)");

            builder.HasMany(p => p.StockMovements)
                   .WithOne(s => s.Product)
                   .HasForeignKey(s => s.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
