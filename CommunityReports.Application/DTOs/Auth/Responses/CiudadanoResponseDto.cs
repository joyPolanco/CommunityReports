namespace CommunityReports.Application.DTOs.Auth.Responses
{
    public sealed class CiudadanoResponseDto : UserResponseDto
    {
        public string Cedula { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Foto { get; set; }
        public short NivelConfiabilidad { get; set; }
    }
}
