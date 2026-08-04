using CommunityReports.Application.DTOs.Instituciones.Requests;
using CommunityReports.Application.DTOs.Instituciones.Responses;

namespace CommunityReports.Application.Interfaces
{
    public interface IInstitucionService
    {
        Task<InstitucionResponseDto> CrearAsync(InstitucionRequestDto request, CancellationToken cancellationToken = default);
        Task<InstitucionResponseDto> ActualizarAsync(int id, InstitucionRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<InstitucionResponseDto>> ListarAsync(CancellationToken cancellationToken = default);
    }
}
