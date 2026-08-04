namespace CommunityReports.Application.DTOs.Auth.Responses
{
    /// <summary>
    /// Respuesta para un usuario con rol Admin. A propósito no tiene campos extra:
    /// Admin es un rol de acceso al sistema (Identity), no una entidad de dominio
    /// como Ciudadano o Empleado, así que no existe un "perfil de Admin" que mapear.
    /// </summary>
    public sealed class AdminResponseDto : UserResponseDto
    {
    }
}
