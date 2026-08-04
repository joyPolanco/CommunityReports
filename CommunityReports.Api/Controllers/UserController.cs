using System.Security.Claims;
using CommunityReports.Application.DTOs.Users.Requests;
using CommunityReports.Application.Interfaces;
using CommunityReports.Domain.Constants;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityReports.Api.Controllers
{
    /// <summary>Perfil y ciclo de vida de la cuenta de usuarios ya autenticados.</summary>
    [ApiController]
    [Route("api/users")]
    [Authorize]
    public class UserController : ControllerBase
    {
        // Compuesto en tiempo de compilación (RoleNames.* son const): habilita el
        // atributo [Authorize(Roles=...)] a Empleado y Admin por igual.
        private const string RolesGestion = $"{RoleNames.Empleado},{RoleNames.Admin}";

        private readonly IUserService _userService;
        private readonly IValidator<ChangePasswordRequestDto> _changePasswordValidator;

        public UserController(IUserService userService, IValidator<ChangePasswordRequestDto> changePasswordValidator)
        {
            _userService = userService;
            _changePasswordValidator = changePasswordValidator;
        }

        /// <summary>Perfil del usuario autenticado (Ciudadano, Empleado o Admin).</summary>
        [HttpGet("me")]
        public async Task<IActionResult> ObtenerPerfilPropio(CancellationToken cancellationToken)
        {
            var usuario = await _userService.ObtenerPerfilAsync(ObtenerUsuarioId(), cancellationToken);
            return Ok(usuario);
        }

        /// <summary>Consulta cualquier usuario por id (ej. un empleado revisando el perfil de un ciudadano que reportó).</summary>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken)
        {
            var usuario = await _userService.ObtenerPerfilAsync(id, cancellationToken);
            return Ok(usuario);
        }

        [HttpGet("ciudadanos")]
        [Authorize(Roles = RolesGestion)]
        public async Task<IActionResult> ListarCiudadanos(CancellationToken cancellationToken)
        {
            var ciudadanos = await _userService.ListarCiudadanosAsync(cancellationToken);
            return Ok(ciudadanos);
        }

        [HttpGet("empleados")]
        [Authorize(Roles = RolesGestion)]
        public async Task<IActionResult> ListarEmpleados(CancellationToken cancellationToken)
        {
            var empleados = await _userService.ListarEmpleadosAsync(cancellationToken);
            return Ok(empleados);
        }

        [HttpPut("me/perfil-ciudadano")]
        public async Task<IActionResult> ActualizarPerfilCiudadano([FromBody] UpdateCiudadanoProfileRequestDto request, CancellationToken cancellationToken)
        {
            var actualizado = await _userService.ActualizarPerfilCiudadanoAsync(ObtenerUsuarioId(), request, cancellationToken);
            return Ok(actualizado);
        }

        [HttpPut("me/perfil-empleado")]
        public async Task<IActionResult> ActualizarPerfilEmpleado([FromBody] UpdateEmpleadoProfileRequestDto request, CancellationToken cancellationToken)
        {
            var actualizado = await _userService.ActualizarPerfilEmpleadoAsync(ObtenerUsuarioId(), request, cancellationToken);
            return Ok(actualizado);
        }

        [HttpPost("me/cambiar-password")]
        public async Task<IActionResult> CambiarPassword([FromBody] ChangePasswordRequestDto request, CancellationToken cancellationToken)
        {
            await _changePasswordValidator.ValidateAndThrowAsync(request, cancellationToken);
            await _userService.CambiarPasswordAsync(ObtenerUsuarioId(), request, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:int}/activar")]
        [Authorize(Roles = RolesGestion)]
        public async Task<IActionResult> Activar(int id, CancellationToken cancellationToken)
        {
            await _userService.ActivarAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpPatch("{id:int}/desactivar")]
        [Authorize(Roles = RolesGestion)]
        public async Task<IActionResult> Desactivar(int id, CancellationToken cancellationToken)
        {
            await _userService.DesactivarAsync(id, cancellationToken);
            return NoContent();
        }

        private int ObtenerUsuarioId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new UnauthorizedAccessException("Token inválido.");

            return int.Parse(claim);
        }
    }
}
