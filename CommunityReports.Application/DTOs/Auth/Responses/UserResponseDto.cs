using System.Text.Json.Serialization;

namespace CommunityReports.Application.DTOs.Auth.Responses
{
    /// <summary>
    /// Base polimórfica para las respuestas de usuario. Se serializa con un
    /// discriminador ("rol") para que el cliente reciba, en un mismo contrato,
    /// los campos propios de Ciudadano o de Empleado según corresponda.
    /// </summary>
    [JsonPolymorphic(TypeDiscriminatorPropertyName = "rol")]
    [JsonDerivedType(typeof(CiudadanoResponseDto), "ciudadano")]
    [JsonDerivedType(typeof(EmpleadoResponseDto), "empleado")]
    [JsonDerivedType(typeof(AdminResponseDto), "admin")]
    public abstract class UserResponseDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimoAcceso { get; set; }
    }
}
