using CommunityReports.Application.DTOs.Categorias.Requests;
using CommunityReports.Application.DTOs.Categorias.Responses;

namespace CommunityReports.Application.Interfaces
{
    public interface ICategoriaService
    {
        Task<CategoriaResponseDto> CrearAsync(CategoriaRequestDto request, CancellationToken cancellationToken = default);
        Task<CategoriaResponseDto> ActualizarAsync(int id, CategoriaRequestDto request, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CategoriaResponseDto>> ListarAsync(CancellationToken cancellationToken = default);
        Task EliminarAsync(int id, CancellationToken cancellationToken = default);
    }
}
