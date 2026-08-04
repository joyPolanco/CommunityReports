using CommunityReports.Application.DTOs.Auth.Responses;
using CommunityReports.Application.DTOs.Identity;
using CommunityReports.Application.DTOs.Users.Requests;
using CommunityReports.Application.Exceptions;
using CommunityReports.Application.Interfaces;
using CommunityReports.Application.Mapping;
using CommunityReports.Domain.Constants;
using CommunityReports.Domain.Interfaces;

namespace CommunityReports.Application.Services
{
    /// <summary>
    /// Casos de uso sobre el perfil y ciclo de vida de la cuenta de un usuario ya
    /// autenticado (o consultado por un administrador/empleado). Combina
    /// <see cref="IIdentityService"/> (cuenta) con los repositorios de dominio
    /// (perfil de Ciudadano/Empleado).
    /// </summary>
    public sealed class UserService : IUserService
    {
        private readonly IIdentityService _identityService;
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IEmpleadoRepository _empleadoRepository;

        public UserService(
            IIdentityService identityService,
            ICiudadanoRepository ciudadanoRepository,
            IEmpleadoRepository empleadoRepository)
        {
            _identityService = identityService;
            _ciudadanoRepository = ciudadanoRepository;
            _empleadoRepository = empleadoRepository;
        }

        public async Task<UserResponseDto> ObtenerPerfilAsync(int usuarioId, CancellationToken cancellationToken = default)
        {
            var usuario = await _identityService.ObtenerPorIdAsync(usuarioId, cancellationToken)
                ?? throw new NotFoundAppException("Usuario no encontrado.");

            return await ResolverPerfilAsync(usuario, cancellationToken);
        }

        public async Task<IReadOnlyList<CiudadanoResponseDto>> ListarCiudadanosAsync(CancellationToken cancellationToken = default)
        {
            var ciudadanos = await _ciudadanoRepository.GetAllAsync(cancellationToken);
            var usuarios = await _identityService.ObtenerVariosPorIdAsync(ciudadanos.Select(c => c.UsuarioId), cancellationToken);

            return ciudadanos
                .Where(c => usuarios.ContainsKey(c.UsuarioId))
                .Select(c => (CiudadanoResponseDto)c.ToResponseDto(usuarios[c.UsuarioId]))
                .ToList();
        }

        public async Task<IReadOnlyList<EmpleadoResponseDto>> ListarEmpleadosAsync(CancellationToken cancellationToken = default)
        {
            var empleados = await _empleadoRepository.GetAllAsync(cancellationToken);
            var usuarios = await _identityService.ObtenerVariosPorIdAsync(empleados.Select(e => e.UsuarioId), cancellationToken);

            return empleados
                .Where(e => usuarios.ContainsKey(e.UsuarioId))
                .Select(e => (EmpleadoResponseDto)e.ToResponseDto(usuarios[e.UsuarioId]))
                .ToList();
        }

        public async Task<CiudadanoResponseDto> ActualizarPerfilCiudadanoAsync(int usuarioId, UpdateCiudadanoProfileRequestDto request, CancellationToken cancellationToken = default)
        {
            var ciudadano = await _ciudadanoRepository.GetByUsuarioIdAsync(usuarioId, cancellationToken)
                ?? throw new NotFoundAppException("Ciudadano no encontrado.");

            ciudadano.ActualizarPerfil(request.Nombres, request.Apellidos, request.Telefono, request.Foto);

            _ciudadanoRepository.Update(ciudadano);
            await _ciudadanoRepository.SaveChangesAsync(cancellationToken);

            var usuario = await _identityService.ObtenerPorIdAsync(usuarioId, cancellationToken)
                ?? throw new NotFoundAppException("Usuario no encontrado.");

            return (CiudadanoResponseDto)ciudadano.ToResponseDto(usuario);
        }

        public async Task<EmpleadoResponseDto> ActualizarPerfilEmpleadoAsync(int usuarioId, UpdateEmpleadoProfileRequestDto request, CancellationToken cancellationToken = default)
        {
            var empleado = await _empleadoRepository.GetByUsuarioIdAsync(usuarioId, cancellationToken)
                ?? throw new NotFoundAppException("Empleado no encontrado.");

            empleado.ActualizarPerfil(request.Cargo, request.Telefono);

            _empleadoRepository.Update(empleado);
            await _empleadoRepository.SaveChangesAsync(cancellationToken);

            var usuario = await _identityService.ObtenerPorIdAsync(usuarioId, cancellationToken)
                ?? throw new NotFoundAppException("Usuario no encontrado.");

            return (EmpleadoResponseDto)empleado.ToResponseDto(usuario);
        }

        public async Task CambiarPasswordAsync(int usuarioId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default)
        {
            var resultado = await _identityService.CambiarPasswordAsync(
                usuarioId, request.PasswordActual, request.PasswordNueva, cancellationToken);

            if (!resultado.Succeeded)
                throw new UnauthorizedAppException(string.Join(" ", resultado.Errors));
        }

        public async Task ActivarAsync(int usuarioId, CancellationToken cancellationToken = default) =>
            await _identityService.ActivarAsync(usuarioId, cancellationToken);

        public async Task DesactivarAsync(int usuarioId, CancellationToken cancellationToken = default) =>
            await _identityService.DesactivarAsync(usuarioId, cancellationToken);

        private async Task<UserResponseDto> ResolverPerfilAsync(UsuarioIdentityDto usuario, CancellationToken cancellationToken)
        {
            if (usuario.TieneRol(RoleNames.Ciudadano))
            {
                var ciudadano = await _ciudadanoRepository.GetByUsuarioIdAsync(usuario.Id, cancellationToken);
                if (ciudadano is not null)
                    return ciudadano.ToResponseDto(usuario);
            }

            if (usuario.TieneRol(RoleNames.Empleado))
            {
                var empleado = await _empleadoRepository.GetByUsuarioIdAsync(usuario.Id, cancellationToken);
                if (empleado is not null)
                    return empleado.ToResponseDto(usuario);
            }

            return usuario.ToAdminResponseDto();
        }
    }
}
