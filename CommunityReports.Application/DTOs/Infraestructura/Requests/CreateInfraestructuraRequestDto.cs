using CommunityReports.Domain.Enums;

namespace CommunityReports.Application.DTOs.Infraestructura.Requests
{
    public sealed class CreateInfraestructuraRequestDto
    {
        public TipoInfraestructura Tipo { get; set; }
        public int DireccionId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
}
