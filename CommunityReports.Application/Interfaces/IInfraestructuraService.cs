using CommunityReports.Application.DTOs.Infraestructura.Requests;
using CommunityReports.Application.DTOs.Infraestructura.Responses;

namespace CommunityReports.Application.Interfaces
{
    public interface IInfraestructuraService
    {
        /// <summary>Nombres del enum TipoInfraestructura, para poblar un selector en el cliente.</summary>
        IReadOnlyList<string> ListarTipos();

        Task<InfraestructuraResponseDto> CrearAsync(CreateInfraestructuraRequestDto request, CancellationToken cancellationToken = default);
        Task<InfraestructuraResponseDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<InfraestructuraResponseDto>> ListarAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<InfraestructuraResponseDto>> ListarPorDireccionAsync(int direccionId, CancellationToken cancellationToken = default);
        Task<InfraestructuraResponseDto> ActualizarAsync(int id, UpdateInfraestructuraRequestDto request, CancellationToken cancellationToken = default);
    }
}
