namespace CommunityReports.Domain.Common
{
    /// <summary>
    /// Base para entidades de catálogo/referencia que son, en esencia, "un nombre"
    /// (Provincia, Municipio, Sector, TipoInfraestructura, Categoria, Estado,
    /// Institucion). Centraliza la validación del nombre para no repetirla en cada
    /// entidad y evita que estas clases sean anémicas (el nombre solo se cambia a
    /// través de un método que valida, nunca con un setter público suelto).
    /// </summary>
    public abstract class CatalogoBase : BaseEntity
    {
        public string Nombre { get; protected set; } = string.Empty;

        protected CatalogoBase()
        {
        }

        protected CatalogoBase(string nombre)
        {
            EstablecerNombre(nombre);
        }

        public void ActualizarNombre(string nombre) => EstablecerNombre(nombre);

        private void EstablecerNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
                throw new ArgumentException("El nombre es obligatorio.", nameof(nombre));

            Nombre = nombre.Trim();
        }
    }
}
