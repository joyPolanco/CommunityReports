namespace CommunityReports.Application.DTOs.Infraestructura.Requests
{
    public sealed class UpdateInfraestructuraRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
