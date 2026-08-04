using CommunityReports.Application.DTOs.Ubicacion.Requests;
using CommunityReports.Application.DTOs.Ubicacion.Responses;

namespace CommunityReports.Application.Interfaces
{
    public interface IUbicacionService
    {
        Task<ProvinciaResponseDto> CrearProvinciaAsync(CreateProvinciaRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ProvinciaResponseDto>> ListarProvinciasAsync(CancellationToken cancellationToken = default);

        Task<MunicipioResponseDto> CrearMunicipioAsync(CreateMunicipioRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<MunicipioResponseDto>> ListarMunicipiosPorProvinciaAsync(int provinciaId, CancellationToken cancellationToken = default);

        Task<SectorResponseDto> CrearSectorAsync(CreateSectorRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<SectorResponseDto>> ListarSectoresPorMunicipioAsync(int municipioId, CancellationToken cancellationToken = default);

        Task<DireccionResponseDto> CrearDireccionAsync(CreateDireccionRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<DireccionResponseDto>> ListarDireccionesPorSectorAsync(int sectorId, CancellationToken cancellationToken = default);
    }
}
