using CommunityReports.Application.DTOs.Auth.Requests;
using CommunityReports.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CommunityReports.Api.Controllers
{
    /// <summary>Registro y autenticación de usuarios (Ciudadano/Empleado).</summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<RegisterCiudadanoRequestDto> _registerCiudadanoValidator;
        private readonly IValidator<RegisterEmpleadoRequestDto> _registerEmpleadoValidator;
        private readonly IValidator<LoginRequestDto> _loginValidator;

        public AuthController(
            IAuthService authService,
            IValidator<RegisterCiudadanoRequestDto> registerCiudadanoValidator,
            IValidator<RegisterEmpleadoRequestDto> registerEmpleadoValidator,
            IValidator<LoginRequestDto> loginValidator)
        {
            _authService = authService;
            _registerCiudadanoValidator = registerCiudadanoValidator;
            _registerEmpleadoValidator = registerEmpleadoValidator;
            _loginValidator = loginValidator;
        }

        /// <summary>Registro público de un ciudadano que reportará incidencias.</summary>
        [HttpPost("register/ciudadano")]
        public async Task<IActionResult> RegistrarCiudadano([FromBody] RegisterCiudadanoRequestDto request, CancellationToken cancellationToken)
        {
            await _registerCiudadanoValidator.ValidateAndThrowAsync(request, cancellationToken);
            var usuario = await _authService.RegistrarCiudadanoAsync(request, cancellationToken);
            return CreatedAtAction(nameof(UserController.ObtenerPorId), "User", new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Registro de un empleado institucional. En producción este endpoint debería
        /// restringirse con [Authorize(Roles = "Empleado")] a un administrador; se deja
        /// abierto aquí para simplificar el flujo del MVP.
        /// </summary>
        [HttpPost("register/empleado")]
        public async Task<IActionResult> RegistrarEmpleado([FromBody] RegisterEmpleadoRequestDto request, CancellationToken cancellationToken)
        {
            await _registerEmpleadoValidator.ValidateAndThrowAsync(request, cancellationToken);
            var usuario = await _authService.RegistrarEmpleadoAsync(request, cancellationToken);
            return CreatedAtAction(nameof(UserController.ObtenerPorId), "User", new { id = usuario.Id }, usuario);
        }

        /// <summary>Autentica un usuario (ciudadano o empleado) y devuelve un JWT.</summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request, CancellationToken cancellationToken)
        {
            await _loginValidator.ValidateAndThrowAsync(request, cancellationToken);
            var respuesta = await _authService.LoginAsync(request, cancellationToken);
            return Ok(respuesta);
        }
    }
}
