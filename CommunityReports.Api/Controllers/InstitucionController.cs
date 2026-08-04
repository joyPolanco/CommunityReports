using CommunityReports.Application.DTOs.Instituciones.Requests;
using CommunityReports.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityReports.Api.Controllers
{
    [ApiController]
    [Route("api/instituciones")]
    public class InstitucionController : ControllerBase
    {
        private readonly IInstitucionService _service;
        private readonly IValidator<InstitucionRequestDto> _validator;

        public InstitucionController(IInstitucionService service, IValidator<InstitucionRequestDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken cancellationToken) =>
            Ok(await _service.ListarAsync(cancellationToken));

        [HttpPost]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Crear([FromBody] InstitucionRequestDto request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            var institucion = await _service.CrearAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Listar), institucion);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] InstitucionRequestDto request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.ActualizarAsync(id, request, cancellationToken));
        }
    }
}
