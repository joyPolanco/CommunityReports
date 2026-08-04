namespace CommunityReports.Application.DTOs.Categorias.Requests
{
    public sealed class CategoriaRequestDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Color { get; set; } = "#808080";
        public int TiempoRespuesta { get; set; }
    }
}
