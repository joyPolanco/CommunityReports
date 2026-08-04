namespace CommunityReports.Application.DTOs.Auth.Responses
{
    public sealed class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiraEn { get; set; }
        public UserResponseDto Usuario { get; set; } = null!;
    }
}
