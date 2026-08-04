namespace CommunityReports.Application.DTOs.Users.Requests
{
    public sealed class UpdateEmpleadoProfileRequestDto
    {
        public string Cargo { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
