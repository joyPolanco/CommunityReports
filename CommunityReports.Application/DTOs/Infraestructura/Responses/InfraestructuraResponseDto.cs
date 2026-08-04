namespace CommunityReports.Application.DTOs.Infraestructura.Responses
{
    public sealed class InfraestructuraResponseDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public int DireccionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
