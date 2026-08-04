using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class DireccionConfiguration : IEntityTypeConfiguration<Direccion>
    {
        public void Configure(EntityTypeBuilder<Direccion> builder)
        {
            builder.ToTable("direccion");
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Id).HasColumnName("id_direccion");
            builder.Property(d => d.SectorId).HasColumnName("id_sector");
            builder.Property(d => d.Calle).HasColumnName("calle").HasMaxLength(200).IsRequired();
            builder.Property(d => d.Referencia).HasColumnName("referencia");
            builder.Property(d => d.CodigoPostal).HasColumnName("codigo_postal").HasMaxLength(20);
            builder.Property(d => d.Latitud).HasColumnName("latitud").HasColumnType("decimal(10,8)");
            builder.Property(d => d.Longitud).HasColumnName("longitud").HasColumnType("decimal(11,8)");

            builder.HasOne(d => d.Sector)
                .WithMany(s => s.Direcciones)
                .HasForeignKey(d => d.SectorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
