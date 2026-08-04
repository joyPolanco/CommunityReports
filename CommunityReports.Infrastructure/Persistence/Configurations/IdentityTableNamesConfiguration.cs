using CommunityReports.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CommunityReports.Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Por defecto Identity crea AspNetUsers, AspNetRoles, AspNetUserRoles, etc.
    /// Se renombran aquí a snake_case en español para que convivan de forma
    /// coherente con "ciudadano", "empleado", "institucion", etc. Esto NO cambia el
    /// comportamiento de Identity, solo los nombres físicos de tabla/columna.
    /// </summary>
    public sealed class IdentityTableNamesConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("usuario");
            builder.Property(u => u.Id).HasColumnName("id_usuario");
            builder.Property(u => u.UserName).HasColumnName("nombre_usuario").HasMaxLength(50);
            builder.Property(u => u.NormalizedUserName).HasColumnName("nombre_usuario_normalizado");
            builder.Property(u => u.Email).HasColumnName("correo").HasMaxLength(120);
            builder.Property(u => u.NormalizedEmail).HasColumnName("correo_normalizado");
            builder.Property(u => u.EmailConfirmed).HasColumnName("correo_confirmado");
            builder.Property(u => u.PasswordHash).HasColumnName("password_hash");
            builder.Property(u => u.PhoneNumber).HasColumnName("telefono");
            builder.Property(u => u.PhoneNumberConfirmed).HasColumnName("telefono_confirmado");
            builder.Property(u => u.TwoFactorEnabled).HasColumnName("doble_factor_habilitado");
            builder.Property(u => u.LockoutEnd).HasColumnName("bloqueo_hasta");
            builder.Property(u => u.LockoutEnabled).HasColumnName("bloqueo_habilitado");
            builder.Property(u => u.AccessFailedCount).HasColumnName("intentos_fallidos");
            builder.Property(u => u.ConcurrencyStamp).HasColumnName("concurrency_stamp");
            builder.Property(u => u.SecurityStamp).HasColumnName("security_stamp");
            builder.Property(u => u.FechaRegistro).HasColumnName("fecha_registro");
            builder.Property(u => u.UltimoAcceso).HasColumnName("ultimo_acceso");
        }
    }

    /// <summary>Renombra AspNetRoles → rol.</summary>
    public sealed class IdentityRoleTableNameConfiguration : IEntityTypeConfiguration<IdentityRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<int>> builder)
        {
            builder.ToTable("rol");
            builder.Property(r => r.Id).HasColumnName("id_rol");
            builder.Property(r => r.Name).HasColumnName("nombre").HasMaxLength(50);
            builder.Property(r => r.NormalizedName).HasColumnName("nombre_normalizado");
            builder.Property(r => r.ConcurrencyStamp).HasColumnName("concurrency_stamp");
        }
    }

    /// <summary>Renombra AspNetUserRoles → usuario_rol.</summary>
    public sealed class IdentityUserRoleTableNameConfiguration : IEntityTypeConfiguration<IdentityUserRole<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<int>> builder)
        {
            builder.ToTable("usuario_rol");
            builder.Property(ur => ur.UserId).HasColumnName("id_usuario");
            builder.Property(ur => ur.RoleId).HasColumnName("id_rol");
        }
    }

    /// <summary>Renombra AspNetUserClaims → usuario_claim.</summary>
    public sealed class IdentityUserClaimTableNameConfiguration : IEntityTypeConfiguration<IdentityUserClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserClaim<int>> builder)
        {
            builder.ToTable("usuario_claim");
            builder.Property(uc => uc.Id).HasColumnName("id");
            builder.Property(uc => uc.UserId).HasColumnName("id_usuario");
        }
    }

    /// <summary>Renombra AspNetRoleClaims → rol_claim.</summary>
    public sealed class IdentityRoleClaimTableNameConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityRoleClaim<int>> builder)
        {
            builder.ToTable("rol_claim");
            builder.Property(rc => rc.Id).HasColumnName("id");
            builder.Property(rc => rc.RoleId).HasColumnName("id_rol");
        }
    }

    /// <summary>Renombra AspNetUserLogins → usuario_login.</summary>
    public sealed class IdentityUserLoginTableNameConfiguration : IEntityTypeConfiguration<IdentityUserLogin<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserLogin<int>> builder)
        {
            builder.ToTable("usuario_login");
            builder.Property(ul => ul.UserId).HasColumnName("id_usuario");
        }
    }

    /// <summary>Renombra AspNetUserTokens → usuario_token.</summary>
    public sealed class IdentityUserTokenTableNameConfiguration : IEntityTypeConfiguration<IdentityUserToken<int>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserToken<int>> builder)
        {
            builder.ToTable("usuario_token");
            builder.Property(ut => ut.UserId).HasColumnName("id_usuario");
        }
    }
}
