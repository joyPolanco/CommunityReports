using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    public class Sector : CatalogoBase
    {
        public int MunicipioId { get; private set; }
        public Municipio? Municipio { get; private set; }

        private readonly List<Direccion> _direcciones = new();
        public IReadOnlyCollection<Direccion> Direcciones => _direcciones.AsReadOnly();

        private Sector()
        {
        }

        public Sector(string nombre, int municipioId) : base(nombre)
        {
            AsignarMunicipio(municipioId);
        }

        public void AsignarMunicipio(int municipioId)
        {
            if (municipioId <= 0)
                throw new ArgumentException("El municipio es obligatorio.", nameof(municipioId));

            MunicipioId = municipioId;
        }
    }
}
