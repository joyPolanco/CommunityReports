namespace CommunityReports.Application.DTOs.Users.Requests
{
    public sealed class ChangePasswordRequestDto
    {
        public string PasswordActual { get; set; } = string.Empty;
        public string PasswordNueva { get; set; } = string.Empty;
    }
}
