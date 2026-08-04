using CommunityReports.Domain.Entities;
using CommunityReports.Infrastructure.Identity;
using CommunityReports.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Persistence
{
    /// <summary>
    /// DbContext de la aplicación. Extiende <see cref="IdentityDbContext{TUser,TRole,TKey}"/>
    /// para que Identity (usuarios, roles, claims, tokens) viva en las mismas
    /// tablas/base de datos que el resto del dominio (territorio, infraestructura,
    /// catálogos e instituciones). "Estado" y "TipoInfraestructura" son enums de
    /// dominio, no tablas (ver Domain.Enums). El módulo de incidencias/reportes se
    /// agregará por separado sin necesidad de modificar lo existente.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Perfiles de dominio de usuarios (Identity maneja la cuenta por separado).
        public DbSet<Ciudadano> Ciudadanos => Set<Ciudadano>();
        public DbSet<Empleado> Empleados => Set<Empleado>();

        // Territorio
        public DbSet<Provincia> Provincias => Set<Provincia>();
        public DbSet<Municipio> Municipios => Set<Municipio>();
        public DbSet<Sector> Sectores => Set<Sector>();
        public DbSet<Direccion> Direcciones => Set<Direccion>();

        // Infraestructura
        public DbSet<Infraestructura> Infraestructuras => Set<Infraestructura>();

        // Catálogos e instituciones
        public DbSet<Categoria> Categorias => Set<Categoria>();
        public DbSet<Institucion> Instituciones => Set<Institucion>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Renombra las tablas de Identity a snake_case en español, coherente
            // con el resto del esquema (usuario, ciudadano, empleado, rol, ...).
            modelBuilder.ApplyConfiguration(new IdentityTableNamesConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityRoleTableNameConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserRoleTableNameConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserClaimTableNameConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityRoleClaimTableNameConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserLoginTableNameConfiguration());
            modelBuilder.ApplyConfiguration(new IdentityUserTokenTableNameConfiguration());

            modelBuilder.ApplyConfiguration(new CiudadanoConfiguration());
            modelBuilder.ApplyConfiguration(new EmpleadoConfiguration());

            modelBuilder.ApplyConfiguration(new ProvinciaConfiguration());
            modelBuilder.ApplyConfiguration(new MunicipioConfiguration());
            modelBuilder.ApplyConfiguration(new SectorConfiguration());
            modelBuilder.ApplyConfiguration(new DireccionConfiguration());

            modelBuilder.ApplyConfiguration(new InfraestructuraConfiguration());

            modelBuilder.ApplyConfiguration(new CategoriaConfiguration());
            modelBuilder.ApplyConfiguration(new InstitucionConfiguration());
        }
    }
}
