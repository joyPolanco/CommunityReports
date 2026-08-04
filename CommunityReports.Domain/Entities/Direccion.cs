using CommunityReports.Domain.Common;

namespace CommunityReports.Domain.Entities
{
    /// <summary>
    /// Dirección física puntual (no es un "catálogo con nombre", por eso no hereda de
    /// CatalogoBase: su identidad es la combinación de calle + coordenadas).
    /// </summary>
    public class Direccion : BaseEntity
    {
        public int SectorId { get; private set; }
        public Sector? Sector { get; private set; }
        public string Calle { get; private set; } = string.Empty;
        public string? Referencia { get; private set; }
        public string? CodigoPostal { get; private set; }
        public decimal? Latitud { get; private set; }
        public decimal? Longitud { get; private set; }

        private readonly List<Infraestructura> _infraestructuras = new();
        public IReadOnlyCollection<Infraestructura> Infraestructuras => _infraestructuras.AsReadOnly();

        private Direccion()
        {
        }

        public Direccion(int sectorId, string calle, string? referencia = null, string? codigoPostal = null,
            decimal? latitud = null, decimal? longitud = null)
        {
            AsignarSector(sectorId);
            EstablecerCalle(calle);
            Referencia = referencia;
            CodigoPostal = codigoPostal;
            EstablecerCoordenadas(latitud, longitud);
        }

        public void AsignarSector(int sectorId)
        {
            if (sectorId <= 0)
                throw new ArgumentException("El sector es obligatorio.", nameof(sectorId));

            SectorId = sectorId;
        }

        public void ActualizarDireccion(string calle, string? referencia, string? codigoPostal)
        {
            EstablecerCalle(calle);
            Referencia = referencia;
            CodigoPostal = codigoPostal;
        }

        public void EstablecerCoordenadas(decimal? latitud, decimal? longitud)
        {
            if (latitud is < -90 or > 90)
                throw new ArgumentException("La latitud debe estar entre -90 y 90.", nameof(latitud));

            if (longitud is < -180 or > 180)
                throw new ArgumentException("La longitud debe estar entre -180 y 180.", nameof(longitud));

            Latitud = latitud;
            Longitud = longitud;
        }

        private void EstablecerCalle(string calle)
        {
            if (string.IsNullOrWhiteSpace(calle))
                throw new ArgumentException("La calle es obligatoria.", nameof(calle));

            Calle = calle.Trim();
        }
    }
}
