namespace CommunityReports.Application.DTOs.Users.Requests
{
    public sealed class UpdateCiudadanoProfileRequestDto
    {
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Foto { get; set; }
    }
}
