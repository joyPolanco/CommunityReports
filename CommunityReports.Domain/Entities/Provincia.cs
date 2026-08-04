using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    /// <summary>Nivel más alto de la jerarquía territorial.</summary>
    public class Provincia : CatalogoBase
    {
        private readonly List<Municipio> _municipios = new();
        public IReadOnlyCollection<Municipio> Municipios => _municipios.AsReadOnly();

        private Provincia()
        {
        }

        public Provincia(string nombre) : base(nombre)
        {
        }
    }
}
