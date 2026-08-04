namespace CommunityReports.Application.DTOs.Instituciones.Requests
{
    public sealed class InstitucionRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Siglas { get; set; }
        public string? Tipo { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? SitioWeb { get; set; }
    }
}
