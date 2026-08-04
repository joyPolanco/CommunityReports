using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    /// <summary>
    /// Perfil de dominio de un empleado de una institución encargado de gestionar
    /// incidencias (tabla "empleado"). Igual que <see cref="Ciudadano"/>, no sabe
    /// nada de autenticación: solo guarda <see cref="UsuarioId"/> como referencia
    /// hacia el usuario de Identity dueño de este perfil.
    /// </summary>
    public class Empleado : BaseEntity
    {
        public int UsuarioId { get; private set; }
        public int InstitucionId { get; private set; }
        public Institucion? Institucion { get; private set; }
        public string Cargo { get; private set; } = string.Empty;
        public string CodigoEmpleado { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }

        private Empleado()
        {
            // Requerido por EF Core.
        }

        public Empleado(
            int usuarioId,
            int institucionId,
            string cargo,
            string codigoEmpleado,
            string? telefono = null)
        {
            AsignarUsuario(usuarioId);
            AsignarInstitucion(institucionId);
            EstablecerCargo(cargo);
            EstablecerCodigoEmpleado(codigoEmpleado);
            Telefono = telefono;
        }

        /// <summary>Un empleado siempre puede gestionar incidencias en nombre de su institución.</summary>
        public bool PuedeGestionarIncidencias() => true;

        public void AsignarInstitucion(int institucionId)
        {
            if (institucionId <= 0)
                throw new ArgumentException("La institución es obligatoria.", nameof(institucionId));

            InstitucionId = institucionId;
        }

        public void ActualizarPerfil(string cargo, string? telefono)
        {
            EstablecerCargo(cargo);
            Telefono = telefono;
        }

        private void AsignarUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("El usuario (Identity) es obligatorio.", nameof(usuarioId));

            UsuarioId = usuarioId;
        }

        private void EstablecerCargo(string cargo)
        {
            if (string.IsNullOrWhiteSpace(cargo))
                throw new ArgumentException("El cargo es obligatorio.", nameof(cargo));

            Cargo = cargo.Trim();
        }

        private void EstablecerCodigoEmpleado(string codigoEmpleado)
        {
            if (string.IsNullOrWhiteSpace(codigoEmpleado))
                throw new ArgumentException("El código de empleado es obligatorio.", nameof(codigoEmpleado));

            CodigoEmpleado = codigoEmpleado.Trim();
        }
    }
}
