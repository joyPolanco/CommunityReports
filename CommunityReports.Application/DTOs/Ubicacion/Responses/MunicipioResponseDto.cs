namespace CommunityReports.Application.DTOs.Ubicacion.Responses
{
    public sealed class MunicipioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int ProvinciaId { get; set; }
    }
}
