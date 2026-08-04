using CommunityReports.Domain.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommunityReports.Infrastructure.Identity
{
    /// <summary>
    /// Se ejecuta una vez al iniciar la Api (ver Program.cs) para garantizar que
    /// existan los 3 roles del sistema (Admin, Ciudadano, Empleado) y, si se
    /// configuró, un usuario Admin inicial. Sin esto, el primer Admin tendría que
    /// crearse manualmente en la base de datos -no hay endpoint público para
    /// registrar Admins, a propósito, porque es un rol de acceso al sistema, no de
    /// auto-registro-.
    /// </summary>
    public static class IdentitySeeder
    {
        public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration, ILogger logger)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var rol in RoleNames.Todos)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    var resultado = await roleManager.CreateAsync(new IdentityRole<int>(rol));
                    if (!resultado.Succeeded)
                        logger.LogWarning("No se pudo crear el rol {Rol}: {Errores}", rol,
                            string.Join(", ", resultado.Errors.Select(e => e.Description)));
                }
            }

            var adminEmail = configuration["Seed:AdminEmail"];
            var adminPassword = configuration["Seed:AdminPassword"];
            var adminUsername = configuration["Seed:AdminUsername"] ?? "admin";

            if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
            {
                logger.LogInformation(
                    "Seed:AdminEmail/Seed:AdminPassword no configurados; no se creó un Admin inicial. " +
                    "Defínelos en appsettings o variables de entorno para sembrar el primer administrador.");
                return;
            }

            if (await userManager.FindByEmailAsync(adminEmail) is not null)
                return;

            var admin = new ApplicationUser
            {
                UserName = adminUsername,
                Email = adminEmail,
                EmailConfirmed = true,
                FechaRegistro = DateTime.UtcNow
            };

            var creado = await userManager.CreateAsync(admin, adminPassword);
            if (!creado.Succeeded)
            {
                logger.LogWarning("No se pudo crear el Admin inicial: {Errores}",
                    string.Join(", ", creado.Errors.Select(e => e.Description)));
                return;
            }

            await userManager.AddToRoleAsync(admin, RoleNames.Admin);
            logger.LogInformation("Admin inicial creado: {Correo}", adminEmail);
        }
    }
}
