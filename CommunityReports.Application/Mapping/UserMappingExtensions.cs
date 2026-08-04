using CommunityReports.Application.DTOs.Auth.Responses;
using CommunityReports.Application.DTOs.Identity;
using CommunityReports.Domain.Entities;

namespace CommunityReports.Application.Mapping
{
    /// <summary>
    /// Combina el perfil de dominio (Ciudadano/Empleado) con los datos de cuenta
    /// que vienen de Identity (<see cref="UsuarioIdentityDto"/>) para producir el DTO
    /// de respuesta. Antes esto era polimorfismo sobre "Usuario"; ahora que
    /// autenticación y dominio están separados, el punto único de combinación son
    /// estos métodos de extensión.
    /// </summary>
    public static class UserMappingExtensions
    {
        public static UserResponseDto ToResponseDto(this Ciudadano ciudadano, UsuarioIdentityDto usuario) =>
            new CiudadanoResponseDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro,
                UltimoAcceso = usuario.UltimoAcceso,
                Cedula = ciudadano.Cedula,
                Nombres = ciudadano.Nombres,
                Apellidos = ciudadano.Apellidos,
                NombreCompleto = ciudadano.NombreCompleto,
                Telefono = ciudadano.Telefono,
                Foto = ciudadano.Foto,
                NivelConfiabilidad = ciudadano.NivelConfiabilidad
            };

        public static UserResponseDto ToResponseDto(this Empleado empleado, UsuarioIdentityDto usuario) =>
            new EmpleadoResponseDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro,
                UltimoAcceso = usuario.UltimoAcceso,
                InstitucionId = empleado.InstitucionId,
                InstitucionNombre = empleado.Institucion?.Nombre,
                Cargo = empleado.Cargo,
                CodigoEmpleado = empleado.CodigoEmpleado,
                Telefono = empleado.Telefono
            };

        /// <summary>Para un usuario con rol Admin, que no tiene perfil de dominio.</summary>
        public static UserResponseDto ToAdminResponseDto(this UsuarioIdentityDto usuario) =>
            new AdminResponseDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Correo = usuario.Correo,
                Estado = usuario.Estado,
                FechaRegistro = usuario.FechaRegistro,
                UltimoAcceso = usuario.UltimoAcceso
            };
    }
}
