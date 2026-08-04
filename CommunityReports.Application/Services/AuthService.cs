using CommunityReports.Application.DTOs.Auth.Requests;
using CommunityReports.Application.DTOs.Auth.Responses;
using CommunityReports.Application.DTOs.Identity;
using CommunityReports.Application.Exceptions;
using CommunityReports.Application.Interfaces;
using CommunityReports.Application.Mapping;
using CommunityReports.Domain.Constants;
using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;

namespace CommunityReports.Application.Services
{
    /// <summary>
    /// Orquesta registro y autenticación. Coordina dos mundos que ahora están
    /// separados a propósito: la cuenta (Identity, vía <see cref="IIdentityService"/>)
    /// y el perfil de dominio (Ciudadano/Empleado, vía sus repositorios). Si el
    /// perfil de dominio falla al crearse justo después del usuario de Identity, se
    /// revierte el usuario para no dejar cuentas huérfanas sin perfil.
    /// </summary>
    public sealed class AuthService : IAuthService
    {
        private readonly IIdentityService _identityService;
        private readonly ICiudadanoRepository _ciudadanoRepository;
        private readonly IEmpleadoRepository _empleadoRepository;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthService(
            IIdentityService identityService,
            ICiudadanoRepository ciudadanoRepository,
            IEmpleadoRepository empleadoRepository,
            IJwtTokenGenerator jwtTokenGenerator)
        {
            _identityService = identityService;
            _ciudadanoRepository = ciudadanoRepository;
            _empleadoRepository = empleadoRepository;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<UserResponseDto> RegistrarCiudadanoAsync(RegisterCiudadanoRequestDto request, CancellationToken cancellationToken = default)
        {
            await ValidarDisponibilidadAsync(request.Correo, request.NombreUsuario, cancellationToken);

            if (await _ciudadanoRepository.ExisteCedulaAsync(request.Cedula, cancellationToken))
                throw new ConflictAppException("La cédula ya está registrada.");

            var creado = await _identityService.CrearUsuarioAsync(
                request.NombreUsuario, request.Correo, request.Password, RoleNames.Ciudadano, cancellationToken);

            if (!creado.Succeeded)
                throw new ConflictAppException(string.Join(" ", creado.Errors));

            var usuarioId = creado.Value!;

            try
            {
                var ciudadano = new Ciudadano(
                    usuarioId,
                    request.Cedula,
                    request.Nombres,
                    request.Apellidos,
                    request.Telefono);

                await _ciudadanoRepository.AddAsync(ciudadano, cancellationToken);
                await _ciudadanoRepository.SaveChangesAsync(cancellationToken);

                var usuario = await _identityService.ObtenerPorIdAsync(usuarioId, cancellationToken)
                    ?? throw new InvalidOperationException("El usuario recién creado no pudo recuperarse.");

                return ciudadano.ToResponseDto(usuario);
            }
            catch
            {
                // Compensación: el perfil de dominio no se pudo crear, no dejamos una cuenta huérfana.
                await _identityService.EliminarUsuarioAsync(usuarioId, cancellationToken);
                throw;
            }
        }

        public async Task<UserResponseDto> RegistrarEmpleadoAsync(RegisterEmpleadoRequestDto request, CancellationToken cancellationToken = default)
        {
            await ValidarDisponibilidadAsync(request.Correo, request.NombreUsuario, cancellationToken);

            if (await _empleadoRepository.ExisteCodigoEmpleadoAsync(request.CodigoEmpleado, cancellationToken))
                throw new ConflictAppException("El código de empleado ya está registrado.");

            var creado = await _identityService.CrearUsuarioAsync(
                request.NombreUsuario, request.Correo, request.Password, RoleNames.Empleado, cancellationToken);

            if (!creado.Succeeded)
                throw new ConflictAppException(string.Join(" ", creado.Errors));

            var usuarioId = creado.Value!;

            try
            {
                var empleado = new Empleado(
                    usuarioId,
                    request.InstitucionId,
                    request.Cargo,
                    request.CodigoEmpleado,
                    request.Telefono);

                await _empleadoRepository.AddAsync(empleado, cancellationToken);
                await _empleadoRepository.SaveChangesAsync(cancellationToken);

                var usuario = await _identityService.ObtenerPorIdAsync(usuarioId, cancellationToken)
                    ?? throw new InvalidOperationException("El usuario recién creado no pudo recuperarse.");

                return empleado.ToResponseDto(usuario);
            }
            catch
            {
                await _identityService.EliminarUsuarioAsync(usuarioId, cancellationToken);
                throw;
            }
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
        {
            var usuario = await _identityService.ValidarCredencialesAsync(request.Correo, request.Password, cancellationToken)
                ?? throw new UnauthorizedAppException("Correo o contraseña incorrectos.");

            if (!usuario.Estado)
                throw new UnauthorizedAppException("La cuenta está desactivada.");

            await _identityService.RegistrarAccesoAsync(usuario.Id, cancellationToken);

            var (token, expiraEn) = _jwtTokenGenerator.GenerarToken(
                new TokenClaimsData(usuario.Id, usuario.NombreUsuario, usuario.Correo, usuario.Roles));

            var perfil = await ResolverPerfilAsync(usuario, cancellationToken);

            return new LoginResponseDto
            {
                Token = token,
                ExpiraEn = expiraEn,
                Usuario = perfil
            };
        }

        /// <summary>
        /// Resuelve el DTO de perfil según el rol del usuario: Ciudadano/Empleado
        /// combinan Identity + dominio; Admin no tiene perfil de dominio.
        /// </summary>
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

        private async Task ValidarDisponibilidadAsync(string correo, string nombreUsuario, CancellationToken cancellationToken)
        {
            if (await _identityService.ExisteCorreoAsync(correo, cancellationToken))
                throw new ConflictAppException("El correo ya está registrado.");

            if (await _identityService.ExisteNombreUsuarioAsync(nombreUsuario, cancellationToken))
                throw new ConflictAppException("El nombre de usuario ya está en uso.");
        }
    }
}
