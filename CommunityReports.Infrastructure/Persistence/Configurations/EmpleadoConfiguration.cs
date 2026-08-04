using CommunityReports.Domain.Entities;
using CommunityReports.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class EmpleadoConfiguration : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> builder)
        {
            builder.ToTable("empleado");

            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).HasColumnName("id_empleado");

            builder.Property(e => e.UsuarioId).HasColumnName("id_usuario").IsRequired();

            builder.Property(e => e.InstitucionId).HasColumnName("id_institucion");

            builder.Property(e => e.Cargo)
                .HasColumnName("cargo")
                .HasMaxLength(80)
                .IsRequired();

            builder.Property(e => e.CodigoEmpleado)
                .HasColumnName("codigo_empleado")
                .HasMaxLength(40)
                .IsRequired();

            builder.Property(e => e.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(20);

            builder.HasIndex(e => e.CodigoEmpleado).IsUnique();

            // Relación 1 a 1 hacia el usuario de Identity (tabla "usuario").
            builder.HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Empleado>(e => e.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.UsuarioId).IsUnique();

            builder.HasOne(e => e.Institucion)
                .WithMany(i => i.Empleados)
                .HasForeignKey(e => e.InstitucionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
