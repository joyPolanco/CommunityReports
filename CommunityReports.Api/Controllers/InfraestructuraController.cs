using CommunityReports.Application.DTOs.Infraestructura.Requests;
using CommunityReports.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityReports.Api.Controllers
{
    [ApiController]
    [Route("api/infraestructuras")]
    public class InfraestructuraController : ControllerBase
    {
        private readonly IInfraestructuraService _service;
        private readonly IValidator<CreateInfraestructuraRequestDto> _createValidator;
        private readonly IValidator<UpdateInfraestructuraRequestDto> _updateValidator;

        public InfraestructuraController(
            IInfraestructuraService service,
            IValidator<CreateInfraestructuraRequestDto> createValidator,
            IValidator<UpdateInfraestructuraRequestDto> updateValidator)
        {
            _service = service;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        /// <summary>Valores válidos de TipoInfraestructura (enum), para poblar un selector.</summary>
        [HttpGet("tipos")]
        public IActionResult ListarTipos() => Ok(_service.ListarTipos());

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken cancellationToken) =>
            Ok(await _service.ListarAsync(cancellationToken));

        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id, CancellationToken cancellationToken) =>
            Ok(await _service.ObtenerPorIdAsync(id, cancellationToken));

        [HttpGet("por-direccion/{direccionId:int}")]
        public async Task<IActionResult> ListarPorDireccion(int direccionId, CancellationToken cancellationToken) =>
            Ok(await _service.ListarPorDireccionAsync(direccionId, cancellationToken));

        [HttpPost]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Crear([FromBody] CreateInfraestructuraRequestDto request, CancellationToken cancellationToken)
        {
            await _createValidator.ValidateAndThrowAsync(request, cancellationToken);
            var infraestructura = await _service.CrearAsync(request, cancellationToken);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = infraestructura.Id }, infraestructura);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] UpdateInfraestructuraRequestDto request, CancellationToken cancellationToken)
        {
            await _updateValidator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.ActualizarAsync(id, request, cancellationToken));
        }
    }
}
