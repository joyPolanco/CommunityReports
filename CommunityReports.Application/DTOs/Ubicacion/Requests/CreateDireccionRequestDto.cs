namespace CommunityReports.Application.DTOs.Ubicacion.Requests
{
    public sealed class CreateDireccionRequestDto
    {
        public int SectorId { get; set; }
        public string Calle { get; set; } = string.Empty;
        public string? Referencia { get; set; }
        public string? CodigoPostal { get; set; }
        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }
    }
}
