namespace CommunityReports.Application.DTOs.Auth.Requests
{
    /// <summary>
    /// Datos para el registro de un Empleado. En un flujo real este endpoint suele
    /// estar restringido a administradores, ya que vincula al usuario a una
    /// institución existente.
    /// </summary>
    public sealed class RegisterEmpleadoRequestDto
    {
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int InstitucionId { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string CodigoEmpleado { get; set; } = string.Empty;
        public string? Telefono { get; set; }
    }
}
