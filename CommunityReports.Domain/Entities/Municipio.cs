using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    public class Municipio : CatalogoBase
    {
        public int ProvinciaId { get; private set; }
        public Provincia? Provincia { get; private set; }

        private readonly List<Sector> _sectores = new();
        public IReadOnlyCollection<Sector> Sectores => _sectores.AsReadOnly();

        private Municipio()
        {
        }

        public Municipio(string nombre, int provinciaId) : base(nombre)
        {
            AsignarProvincia(provinciaId);
        }

        public void AsignarProvincia(int provinciaId)
        {
            if (provinciaId <= 0)
                throw new ArgumentException("La provincia es obligatoria.", nameof(provinciaId));

            ProvinciaId = provinciaId;
        }
    }
}
