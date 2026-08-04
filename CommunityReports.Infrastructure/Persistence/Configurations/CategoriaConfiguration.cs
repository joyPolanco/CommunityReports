using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class CategoriaConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categoria");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id_categoria");
            builder.Property(c => c.Nombre).HasColumnName("nombre").HasMaxLength(80).IsRequired();
            builder.Property(c => c.Color).HasColumnName("color").HasMaxLength(20);
            builder.Property(c => c.TiempoRespuesta).HasColumnName("tiempo_respuesta");
            builder.HasIndex(c => c.Nombre).IsUnique();
        }
    }
}
