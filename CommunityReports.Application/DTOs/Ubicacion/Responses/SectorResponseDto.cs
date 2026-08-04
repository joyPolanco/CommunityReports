namespace CommunityReports.Application.DTOs.Ubicacion.Responses
{
    public sealed class SectorResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int MunicipioId { get; set; }
    }
}
