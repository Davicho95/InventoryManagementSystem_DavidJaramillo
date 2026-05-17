using Dominio.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistencia.Contexto.Configuraciones
{
    internal class StockMovementConfig : IEntityTypeConfiguration<StockMovement>
    {
        public void Configure(EntityTypeBuilder<StockMovement> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Type)
                   .HasConversion<string>()
                   .IsRequired();

            builder.Property(s => s.Reason)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(s => s.Timestamp)
                   .IsRequired();
        }
    }
}
