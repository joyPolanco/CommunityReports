namespace CommunityReports.Application.DTOs.Ubicacion.Requests
{
    public sealed class CreateSectorRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public int MunicipioId { get; set; }
    }
}
