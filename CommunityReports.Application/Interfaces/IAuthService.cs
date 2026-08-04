using CommunityReports.Application.DTOs.Auth.Requests;
using CommunityReports.Application.DTOs.Auth.Responses;

namespace CommunityReports.Application.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> RegistrarCiudadanoAsync(RegisterCiudadanoRequestDto request, CancellationToken cancellationToken = default);
        Task<UserResponseDto> RegistrarEmpleadoAsync(RegisterEmpleadoRequestDto request, CancellationToken cancellationToken = default);
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    }
}
