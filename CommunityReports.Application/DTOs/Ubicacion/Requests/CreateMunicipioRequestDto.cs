namespace CommunityReports.Application.DTOs.Ubicacion.Requests
{
    public sealed class CreateMunicipioRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int ProvinciaId { get; set; }
    }
}
