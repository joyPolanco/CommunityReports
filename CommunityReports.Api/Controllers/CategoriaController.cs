using CommunityReports.Application.DTOs.Categorias.Requests;
using CommunityReports.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityReports.Api.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _service;
        private readonly IValidator<CategoriaRequestDto> _validator;

        public CategoriaController(ICategoriaService service, IValidator<CategoriaRequestDto> validator)
        {
            _service = service;
            _validator = validator;
        }

        [HttpGet]
        public async Task<IActionResult> Listar(CancellationToken cancellationToken) =>
            Ok(await _service.ListarAsync(cancellationToken));

        [HttpPost]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Crear([FromBody] CategoriaRequestDto request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            var categoria = await _service.CrearAsync(request, cancellationToken);
            return CreatedAtAction(nameof(Listar), categoria);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CategoriaRequestDto request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.ActualizarAsync(id, request, cancellationToken));
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
        {
            await _service.EliminarAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
