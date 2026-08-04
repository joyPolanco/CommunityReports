using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class InfraestructuraConfiguration : IEntityTypeConfiguration<Infraestructura>
    {
        public void Configure(EntityTypeBuilder<Infraestructura> builder)
        {
            builder.ToTable("infraestructura");
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("id_infraestructura");

            // El tipo es un enum (ver Domain.Enums.TipoInfraestructura); se guarda
            // como texto para que la columna sea legible directamente en la BD.
            builder.Property(i => i.Tipo)
                .HasColumnName("tipo")
                .HasConversion<string>()
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(i => i.DireccionId).HasColumnName("id_direccion");
            builder.Property(i => i.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            builder.Property(i => i.Codigo).HasColumnName("codigo").HasMaxLength(40).IsRequired();
            builder.Property(i => i.Descripcion).HasColumnName("descripcion");

            builder.HasIndex(i => i.Codigo).IsUnique();

            builder.HasOne(i => i.Direccion)
                .WithMany(d => d.Infraestructuras)
                .HasForeignKey(i => i.DireccionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
