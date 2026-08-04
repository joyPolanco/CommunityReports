using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    /// <summary>
    /// Perfil de dominio de un ciudadano que reporta incidencias comunitarias
    /// (tabla "ciudadano"). No contiene nada de autenticación: correo, contraseña,
    /// nombre de usuario y estado de cuenta viven en Identity (ApplicationUser, en
    /// Infrastructure). <see cref="UsuarioId"/> es la única conexión hacia ese
    /// usuario, guardada como un simple valor de FK -el dominio conoce el id, no
    /// conoce Identity-.
    /// </summary>
    public class Ciudadano : BaseEntity
    {
        public const short NivelConfiabilidadMinimo = 1;
        public const short NivelConfiabilidadMaximo = 5;
        private const short NivelConfiabilidadInicial = 3;

        public int UsuarioId { get; private set; }
        public string Cedula { get; private set; } = string.Empty;
        public string Nombres { get; private set; } = string.Empty;
        public string Apellidos { get; private set; } = string.Empty;
        public string? Telefono { get; private set; }
        public string? Foto { get; private set; }
        public short NivelConfiabilidad { get; private set; } = NivelConfiabilidadInicial;

        public string NombreCompleto => $"{Nombres} {Apellidos}".Trim();

        private Ciudadano()
        {
            // Requerido por EF Core.
        }

        public Ciudadano(
            int usuarioId,
            string cedula,
            string nombres,
            string apellidos,
            string? telefono = null)
        {
            AsignarUsuario(usuarioId);
            EstablecerCedula(cedula);
            EstablecerNombres(nombres, apellidos);
            Telefono = telefono;
            NivelConfiabilidad = NivelConfiabilidadInicial;
        }

        /// <summary>Un ciudadano nunca gestiona incidencias reportadas por otros; solo las reporta.</summary>
        public bool PuedeGestionarIncidencias() => false;

        public void ActualizarPerfil(string nombres, string apellidos, string? telefono, string? foto)
        {
            EstablecerNombres(nombres, apellidos);
            Telefono = telefono;
            Foto = foto;
        }

        /// <summary>
        /// Aumenta la confiabilidad del ciudadano (por ejemplo cuando un empleado
        /// valida un reporte como legítimo). Queda acotada entre 1 y 5.
        /// </summary>
        public void AumentarConfiabilidad(short puntos = 1)
            => NivelConfiabilidad = (short)Math.Min(NivelConfiabilidadMaximo, NivelConfiabilidad + puntos);

        /// <summary>
        /// Disminuye la confiabilidad del ciudadano (por ejemplo cuando un reporte se
        /// valida como falso). Queda acotada entre 1 y 5.
        /// </summary>
        public void DisminuirConfiabilidad(short puntos = 1)
            => NivelConfiabilidad = (short)Math.Max(NivelConfiabilidadMinimo, NivelConfiabilidad - puntos);

        private void AsignarUsuario(int usuarioId)
        {
            if (usuarioId <= 0)
                throw new ArgumentException("El usuario (Identity) es obligatorio.", nameof(usuarioId));

            UsuarioId = usuarioId;
        }

        private void EstablecerCedula(string cedula)
        {
            if (string.IsNullOrWhiteSpace(cedula))
                throw new ArgumentException("La cédula es obligatoria.", nameof(cedula));

            Cedula = cedula.Trim();
        }

        private void EstablecerNombres(string nombres, string apellidos)
        {
            if (string.IsNullOrWhiteSpace(nombres))
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombres));

            if (string.IsNullOrWhiteSpace(apellidos))
                throw new ArgumentException("El apellido es obligatorio.", nameof(apellidos));

            Nombres = nombres.Trim();
            Apellidos = apellidos.Trim();
        }
    }
}
