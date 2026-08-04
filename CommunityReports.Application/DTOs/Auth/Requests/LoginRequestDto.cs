namespace CommunityReports.Application.DTOs.Auth.Requests
{
    public sealed class LoginRequestDto
    {
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
