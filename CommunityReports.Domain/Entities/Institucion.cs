using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    /// <summary>
    /// Institución responsable de atender incidencias (ayuntamiento, acueducto,
    /// EDESUR, etc.). Hereda de CatalogoBase por su Nombre; el resto de campos de
    /// contacto son propios.
    /// </summary>
    public class Institucion : CatalogoBase
    {
        public string? Siglas { get; private set; }
        public string? Tipo { get; private set; }
        public string? Telefono { get; private set; }
        public string? Correo { get; private set; }
        public string? SitioWeb { get; private set; }

        private readonly List<Empleado> _empleados = new();
        public IReadOnlyCollection<Empleado> Empleados => _empleados.AsReadOnly();

        private Institucion()
        {
        }

        public Institucion(string nombre, string? siglas = null, string? tipo = null, string? telefono = null,
            string? correo = null, string? sitioWeb = null) : base(nombre)
        {
            Siglas = siglas;
            Tipo = tipo;
            Telefono = telefono;
            Correo = correo;
            SitioWeb = sitioWeb;
        }

        public void ActualizarDatos(string nombre, string? siglas, string? tipo, string? telefono, string? correo, string? sitioWeb)
        {
            ActualizarNombre(nombre);
            Siglas = siglas;
            Tipo = tipo;
            Telefono = telefono;
            Correo = correo;
            SitioWeb = sitioWeb;
        }
    }
}
