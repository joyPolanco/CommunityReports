using CommunityReports.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    public class ProvinciaConfiguration : IEntityTypeConfiguration<Provincia>
    {
        public void Configure(EntityTypeBuilder<Provincia> builder)
        {
            builder.ToTable("provincia");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("id_provincia");
            builder.Property(p => p.Nombre).HasColumnName("nombre").HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.Nombre).IsUnique();
        }
    }
}
