using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class SectorConfiguration : IEntityTypeConfiguration<Sector>
    {
        public void Configure(EntityTypeBuilder<Sector> builder)
        {
            builder.ToTable("sector");
            builder.HasKey(s => s.Id);
            builder.Property(s => s.Id).HasColumnName("id_sector");
            builder.Property(s => s.MunicipioId).HasColumnName("id_municipio");
            builder.Property(s => s.Nombre).HasColumnName("nombre").HasMaxLength(120).IsRequired();

            builder.HasOne(s => s.Municipio)
                .WithMany(m => m.Sectores)
                .HasForeignKey(s => s.MunicipioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
