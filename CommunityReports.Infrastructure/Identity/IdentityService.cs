using CommunityReports.Application.DTOs.Identity;
using CommunityReports.Application.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Identity
{
    /// <summary>
    /// Implementa <see cref="IIdentityService"/> sobre ASP.NET Core Identity. Es el
    /// único lugar de la solución (fuera de este mismo namespace) donde se usan
    /// <see cref="UserManager{TUser}"/> y <see cref="ApplicationUser"/> directamente;
    /// Application solo ve <see cref="UsuarioIdentityDto"/> e
    /// <see cref="IdentityOperationResult"/>.
    /// </summary>
    public sealed class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public IdentityService(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IdentityOperationResult<int>> CrearUsuarioAsync(
            string nombreUsuario, string correo, string password, string rol, CancellationToken cancellationToken = default)
        {
            var user = new ApplicationUser
            {
                UserName = nombreUsuario,
                Email = correo,
                EmailConfirmed = true, // MVP: sin flujo de confirmación de correo por ahora.
                FechaRegistro = DateTime.UtcNow
            };

            var creado = await _userManager.CreateAsync(user, password);
            if (!creado.Succeeded)
                return IdentityOperationResult<int>.Fail(creado.Errors.Select(e => e.Description));

            var asignado = await _userManager.AddToRoleAsync(user, rol);
            if (!asignado.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return IdentityOperationResult<int>.Fail(asignado.Errors.Select(e => e.Description));
            }

            return IdentityOperationResult<int>.Ok(user.Id);
        }

        public async Task EliminarUsuarioAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            if (user is not null)
                await _userManager.DeleteAsync(user);
        }

        public async Task<UsuarioIdentityDto?> ValidarCredencialesAsync(string correo, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByEmailAsync(correo);
            if (user is null)
                return null;

            var passwordValida = await _userManager.CheckPasswordAsync(user, password);
            if (!passwordValida)
                return null;

            return await MapearAsync(user);
        }

        public async Task<UsuarioIdentityDto?> ObtenerPorIdAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            return user is null ? null : await MapearAsync(user);
        }

        public async Task<IReadOnlyDictionary<int, UsuarioIdentityDto>> ObtenerVariosPorIdAsync(
            IEnumerable<int> usuarioIds, CancellationToken cancellationToken = default)
        {
            var ids = usuarioIds.Distinct().ToList();
            if (ids.Count == 0)
                return new Dictionary<int, UsuarioIdentityDto>();

            var usuarios = await _context.Users
                .Where(u => ids.Contains(u.Id))
                .ToListAsync(cancellationToken);

            var rolesPorUsuario = await (
                from ur in _context.UserRoles
                join r in _context.Roles on ur.RoleId equals r.Id
                where ids.Contains(ur.UserId)
                select new { ur.UserId, RoleName = r.Name }
            ).ToListAsync(cancellationToken);

            var rolesLookup = rolesPorUsuario
                .GroupBy(x => x.UserId)
                .ToDictionary(g => g.Key, g => (IReadOnlyList<string>)g.Select(x => x.RoleName ?? string.Empty).ToList());

            return usuarios.ToDictionary(u => u.Id, u => MapearDto(u, rolesLookup.GetValueOrDefault(u.Id, Array.Empty<string>())));
        }

        public async Task<bool> ExisteCorreoAsync(string correo, CancellationToken cancellationToken = default) =>
            await _userManager.FindByEmailAsync(correo) is not null;

        public async Task<bool> ExisteNombreUsuarioAsync(string nombreUsuario, CancellationToken cancellationToken = default) =>
            await _userManager.FindByNameAsync(nombreUsuario) is not null;

        public async Task<IdentityOperationResult> CambiarPasswordAsync(
            int usuarioId, string passwordActual, string passwordNueva, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            if (user is null)
                return IdentityOperationResult.Fail("Usuario no encontrado.");

            var resultado = await _userManager.ChangePasswordAsync(user, passwordActual, passwordNueva);
            return resultado.Succeeded
                ? IdentityOperationResult.Ok()
                : IdentityOperationResult.Fail(resultado.Errors.Select(e => e.Description));
        }

        public async Task ActivarAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            if (user is null)
                return;

            await _userManager.SetLockoutEndDateAsync(user, null);
        }

        public async Task DesactivarAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            if (user is null)
                return;

            if (!user.LockoutEnabled)
                await _userManager.SetLockoutEnabledAsync(user, true);

            await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
        }

        public async Task RegistrarAccesoAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var user = await _userManager.FindByIdAsync(usuarioId.ToString());
            if (user is null)
                return;

            user.UltimoAcceso = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);
        }

        private async Task<UsuarioIdentityDto> MapearAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            return MapearDto(user, roles.ToList());
        }

        private static UsuarioIdentityDto MapearDto(ApplicationUser user, IReadOnlyList<string> roles) => new()
        {
            Id = user.Id,
            NombreUsuario = user.UserName ?? string.Empty,
            Correo = user.Email ?? string.Empty,
            Estado = !EstaBloqueado(user),
            FechaRegistro = user.FechaRegistro,
            UltimoAcceso = user.UltimoAcceso,
            Roles = roles
        };

        private static bool EstaBloqueado(ApplicationUser user) =>
            user.LockoutEnd.HasValue && user.LockoutEnd.Value > DateTimeOffset.UtcNow;
    }
}
