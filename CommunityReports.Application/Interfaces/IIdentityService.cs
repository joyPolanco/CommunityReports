using CommunityReports.Application.DTOs.Identity;
using CommunityReports.Domain.Constants;

namespace CommunityReports.Application.Interfaces
{
    /// <summary>
    /// Puerto hacia el proveedor de identidad (ASP.NET Core Identity, implementado
    /// en Infrastructure). Concentra TODO lo de autenticación/cuenta -crear
    /// usuarios, validar credenciales, roles, activar/desactivar, cambiar
    /// contraseña- para que Application y Domain nunca dependan de
    /// Microsoft.AspNetCore.Identity ni conozcan ApplicationUser.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>
        /// Crea el usuario de Identity y le asigna el rol indicado (Admin,
        /// Ciudadano o Empleado; ver <see cref="Domain.Constants.RoleNames"/>).
        /// Devuelve el id generado en <see cref="IdentityOperationResult{T}.Value"/>.
        /// </summary>
        Task<IdentityOperationResult<int>> CrearUsuarioAsync(
            string nombreUsuario, string correo, string password, string rol, CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina un usuario de Identity. Se usa como acción compensatoria cuando
        /// falla la creación del perfil de dominio (Ciudadano/Empleado) justo
        /// después de crear el usuario, para no dejar cuentas huérfanas sin perfil.
        /// </summary>
        Task EliminarUsuarioAsync(int usuarioId, CancellationToken cancellationToken = default);

        /// <summary>Valida correo + contraseña. Devuelve null si son inválidos o la cuenta está bloqueada por intentos fallidos.</summary>
        Task<UsuarioIdentityDto?> ValidarCredencialesAsync(string correo, string password, CancellationToken cancellationToken = default);

        Task<UsuarioIdentityDto?> ObtenerPorIdAsync(int usuarioId, CancellationToken cancellationToken = default);

        /// <summary>Trae varios usuarios de una vez (evita N llamadas al listar ciudadanos/empleados).</summary>
        Task<IReadOnlyDictionary<int, UsuarioIdentityDto>> ObtenerVariosPorIdAsync(IEnumerable<int> usuarioIds, CancellationToken cancellationToken = default);

        Task<bool> ExisteCorreoAsync(string correo, CancellationToken cancellationToken = default);
        Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default);

        Task<IdentityOperationResult> CambiarPasswordAsync(
            int usuarioId, string passwordActual, string passwordNueva, CancellationToken cancellationToken = default);

        /// <summary>Reactiva la cuenta (levanta el bloqueo).</summary>
        Task ActivarAsync(int usuarioId, CancellationToken cancellationToken = default);

        /// <summary>Desactiva la cuenta (bloqueo indefinido; ya no puede iniciar sesión).</summary>
        Task DesactivarAsync(int usuarioId, CancellationToken cancellationToken = default);

        /// <summary>Registra la fecha del último acceso exitoso.</summary>
        Task RegistrarAccesoAsync(int usuarioId, CancellationToken cancellationToken = default);
    }
}
