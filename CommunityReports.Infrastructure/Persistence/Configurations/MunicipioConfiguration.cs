using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class MunicipioConfiguration : IEntityTypeConfiguration<Municipio>
    {
        public void Configure(EntityTypeBuilder<Municipio> builder)
        {
            builder.ToTable("municipio");
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id).HasColumnName("id_municipio");
            builder.Property(m => m.ProvinciaId).HasColumnName("id_provincia");
            builder.Property(m => m.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();

            builder.HasOne(m => m.Provincia)
                .WithMany(p => p.Municipios)
                .HasForeignKey(m => m.ProvinciaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
