using CommunityReports.Domain.Common;
using CommunityReports.Domain.Enums;

namespace CommunityReports.Domain.Entities
{
    /// <summary>
    /// Activo físico concreto (un poste, un tramo de acueducto, etc.) ubicado en una
    /// Direccion y clasificado por <see cref="TipoInfraestructura"/> (enum, no
    /// tabla: ver comentario en el enum).
    /// </summary>
    public class Infraestructura : BaseEntity
    {
        public TipoInfraestructura Tipo { get; private set; }
        public int DireccionId { get; private set; }
        public Direccion? Direccion { get; private set; }
        public string Nombre { get; private set; } = string.Empty;
        public string Codigo { get; private set; } = string.Empty;
        public string? Descripcion { get; private set; }

        private Infraestructura()
        {
        }

        public Infraestructura(TipoInfraestructura tipo, int direccionId, string nombre, string codigo, string? descripcion = null)
        {
            Tipo = tipo;
            AsignarDireccion(direccionId);
            EstablecerNombre(nombre);
            EstablecerCodigo(codigo);
            Descripcion = descripcion;
        }

        public void AsignarDireccion(int direccionId)
        {
            if (direccionId <= 0)
                throw new ArgumentException("La dirección es obligatoria.", nameof(direccionId));

            DireccionId = direccionId;
        }

        public void ActualizarDatos(string nombre, string? descripcion)
        {
            EstablecerNombre(nombre);
            Descripcion = descripcion;
        }

        public void Reclasificar(TipoInfraestructura tipo) => Tipo = tipo;

        private void EstablecerNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));

            Nombre = nombre.Trim();
        }

        private void EstablecerCodigo(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
                throw new ArgumentException("El código es obligatorio.", nameof(codigo));

            Codigo = codigo.Trim();
        }
    }
}
