using CommunityReports.Application.DTOs.Auth.Responses;
using CommunityReports.Application.DTOs.Users.Requests;

namespace CommunityReports.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> ObtenerPerfilAsync(int usuarioId, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<CiudadanoResponseDto>> ListarCiudadanosAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<EmpleadoResponseDto>> ListarEmpleadosAsync(CancellationToken cancellationToken = default);

        Task<CiudadanoResponseDto> ActualizarPerfilCiudadanoAsync(int usuarioId, UpdateCiudadanoProfileRequestDto request, CancellationToken cancellationToken = default);
        Task<EmpleadoResponseDto> ActualizarPerfilEmpleadoAsync(int usuarioId, UpdateEmpleadoProfileRequestDto request, CancellationToken cancellationToken = default);

        Task CambiarPasswordAsync(int usuarioId, ChangePasswordRequestDto request, CancellationToken cancellationToken = default);
        Task ActivarAsync(int usuarioId, CancellationToken cancellationToken = default);
        Task DesactivarAsync(int usuarioId, CancellationToken cancellationToken = default);
    }
}
