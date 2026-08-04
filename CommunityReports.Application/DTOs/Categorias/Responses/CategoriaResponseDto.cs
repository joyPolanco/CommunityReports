namespace CommunityReports.Application.DTOs.Categorias.Responses
{
    public sealed class CategoriaResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public int TiempoRespuesta { get; set; }
    }
}
