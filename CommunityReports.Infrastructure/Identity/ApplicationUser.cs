using Microsoft.AspNetCore.Identity;

namespace CommunityReports.Infrastructure.Identity
{
    /// <summary>
    /// Usuario de Identity (tabla "usuario", antes AspNetUsers). Concentra TODO lo
    /// de autenticación: <c>UserName</c>, <c>Email</c>, <c>PasswordHash</c> (hasheado
    /// por Identity, ya no por un IPasswordHasher propio), y el bloqueo de cuenta
    /// (<c>LockoutEnabled</c>/<c>LockoutEnd</c>) que usamos para Activar/Desactivar.
    /// Ciudadano y Empleado NO heredan de esta clase: solo guardan su
    /// <c>UsuarioId</c> como referencia. Este es el único punto de la solución que
    /// conoce Microsoft.AspNetCore.Identity fuera de Infrastructure/Identity.
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoAcceso { get; set; }
    }
}
