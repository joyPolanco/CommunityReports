using CommunityReports.Domain.Entities;
using CommunityReports.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class CiudadanoConfiguration : IEntityTypeConfiguration<Ciudadano>
    {
        public void Configure(EntityTypeBuilder<Ciudadano> builder)
        {
            builder.ToTable("ciudadano");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id_ciudadano");

            builder.Property(c => c.UsuarioId).HasColumnName("id_usuario").IsRequired();

            builder.Property(c => c.Cedula)
                .HasColumnName("cedula")
                .HasMaxLength(15)
                .IsRequired();

            builder.Property(c => c.Nombres)
                .HasColumnName("nombres")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(c => c.Apellidos)
                .HasColumnName("apellidos")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(c => c.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            builder.Property(c => c.Foto)
                .HasColumnName("foto");

            builder.Property(c => c.NivelConfiabilidad)
                .HasColumnName("nivel_confiabilidad")
                .HasDefaultValue((short)3);

            builder.HasIndex(c => c.Cedula).IsUnique();

            // Relación 1 a 1 hacia el usuario de Identity (tabla "usuario"). No hay
            // navegación de vuelta desde ApplicationUser: Identity no necesita saber
            // que existe un Ciudadano, solo la FK vive en este lado.
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Ciudadano>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(c => c.UsuarioId).IsUnique();

            builder.Ignore(c => c.NombreCompleto);
        }
    }
}
