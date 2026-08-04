using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class InstitucionConfiguration : IEntityTypeConfiguration<Institucion>
    {
        public void Configure(EntityTypeBuilder<Institucion> builder)
        {
            builder.ToTable("institucion");

            builder.HasKey(i => i.Id);
            builder.Property(i => i.Id).HasColumnName("id_institucion");

            builder.Property(i => i.Nombre).HasColumnName("nombre").HasMaxLength(150).IsRequired();
            builder.Property(i => i.Siglas).HasColumnName("siglas").HasMaxLength(30);
            builder.Property(i => i.Tipo).HasColumnName("tipo").HasMaxLength(60);
            builder.Property(i => i.Telefono).HasColumnName("telefono").HasMaxLength(20);
            builder.Property(i => i.Correo).HasColumnName("correo").HasMaxLength(120);
            builder.Property(i => i.SitioWeb).HasColumnName("sitio_web").HasMaxLength(200);

            builder.HasIndex(i => i.Nombre).IsUnique();
        }
    }
}
