using CommunityReports.Application.DTOs.Ubicacion.Requests;
using CommunityReports.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CommunityReports.Api.Controllers
{
    /// <summary>Jerarquía territorial: provincia → municipio → sector → dirección.</summary>
    [ApiController]
    [Route("api/ubicaciones")]
    public class UbicacionController : ControllerBase
    {
        private readonly IUbicacionService _service;
        private readonly IValidator<CreateProvinciaRequestDto> _provinciaValidator;
        private readonly IValidator<CreateMunicipioRequestDto> _municipioValidator;
        private readonly IValidator<CreateSectorRequestDto> _sectorValidator;
        private readonly IValidator<CreateDireccionRequestDto> _direccionValidator;

        public UbicacionController(
            IUbicacionService service,
            IValidator<CreateProvinciaRequestDto> provinciaValidator,
            IValidator<CreateMunicipioRequestDto> municipioValidator,
            IValidator<CreateSectorRequestDto> sectorValidator,
            IValidator<CreateDireccionRequestDto> direccionValidator)
        {
            _service = service;
            _provinciaValidator = provinciaValidator;
            _municipioValidator = municipioValidator;
            _sectorValidator = sectorValidator;
            _direccionValidator = direccionValidator;
        }

        [HttpGet("provincias")]
        public async Task<IActionResult> ListarProvincias(CancellationToken cancellationToken) =>
            Ok(await _service.ListarProvinciasAsync(cancellationToken));

        [HttpPost("provincias")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> CrearProvincia([FromBody] CreateProvinciaRequestDto request, CancellationToken cancellationToken)
        {
            await _provinciaValidator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.CrearProvinciaAsync(request, cancellationToken));
        }

        [HttpGet("provincias/{provinciaId:int}/municipios")]
        public async Task<IActionResult> ListarMunicipios(int provinciaId, CancellationToken cancellationToken) =>
            Ok(await _service.ListarMunicipiosPorProvinciaAsync(provinciaId, cancellationToken));

        [HttpPost("municipios")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> CrearMunicipio([FromBody] CreateMunicipioRequestDto request, CancellationToken cancellationToken)
        {
            await _municipioValidator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.CrearMunicipioAsync(request, cancellationToken));
        }

        [HttpGet("municipios/{municipioId:int}/sectores")]
        public async Task<IActionResult> ListarSectores(int municipioId, CancellationToken cancellationToken) =>
            Ok(await _service.ListarSectoresPorMunicipioAsync(municipioId, cancellationToken));

        [HttpPost("sectores")]
        [Authorize(Roles = "Empleado")]
        public async Task<IActionResult> CrearSector([FromBody] CreateSectorRequestDto request, CancellationToken cancellationToken)
        {
            await _sectorValidator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.CrearSectorAsync(request, cancellationToken));
        }

        [HttpGet("sectores/{sectorId:int}/direcciones")]
        public async Task<IActionResult> ListarDirecciones(int sectorId, CancellationToken cancellationToken) =>
            Ok(await _service.ListarDireccionesPorSectorAsync(sectorId, cancellationToken));

        [HttpPost("direcciones")]
        public async Task<IActionResult> CrearDireccion([FromBody] CreateDireccionRequestDto request, CancellationToken cancellationToken)
        {
            await _direccionValidator.ValidateAndThrowAsync(request, cancellationToken);
            return Ok(await _service.CrearDireccionAsync(request, cancellationToken));
        }
    }
}
