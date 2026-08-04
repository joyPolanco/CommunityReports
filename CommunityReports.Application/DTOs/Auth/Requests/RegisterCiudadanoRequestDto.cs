namespace CommunityReports.Application.DTOs.Auth.Requests
{
    /// <summary>Datos para el registro público de un Ciudadano.</summary>
    public sealed class RegisterCiudadanoRequestDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Cedula { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
