namespace CommunityReports.Application.DTOs.Identity
{
    /// <summary>
    /// Representa, desde el punto de vista de Application, al usuario de Identity
    /// (tabla "usuario" de AspNetUsers). Es la única forma en que Application "ve"
    /// datos de autenticación: nunca referencia ApplicationUser, UserManager ni
    /// ningún tipo de Microsoft.AspNetCore.Identity directamente.
    /// </summary>
    public sealed class UsuarioIdentityDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;

        /// <summary>Cuenta activa (true) o desactivada/bloqueada (false).</summary>
        public bool Estado { get; set; }

        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimoAcceso { get; set; }

        /// <summary>Roles asignados (Admin, Ciudadano o Empleado).</summary>
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();

        public bool TieneRol(string rol) => Roles.Contains(rol, StringComparer.OrdinalIgnoreCase);
    }
}
