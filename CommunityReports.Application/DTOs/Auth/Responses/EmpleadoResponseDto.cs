namespace CommunityReports.Application.DTOs.Auth.Responses
{
    public sealed class EmpleadoResponseDto : UserResponseDto
    {
        public int InstitucionId { get; set; }
        public string? InstitucionNombre { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string CodigoEmpleado { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
