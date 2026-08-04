using CommunityReports.Domain.Entities;

namespace CommunityReports.Domain.Interfaces
{
    /// <summary>
    /// Repositorio unificado para la jerarquía territorial (Provincia → Municipio →
    /// Sector → Dirección). Se unifica en una sola interfaz porque las cuatro
    /// entidades siempre se consultan/mutan juntas dentro del mismo caso de uso
    /// ("dar de alta una dirección"); separarlas en cuatro repositorios no aportaría
    /// valor para este MVP.
    /// </summary>
    public interface IUbicacionRepository
    {
        Task<Provincia?> GetProvinciaByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Provincia>> GetProvinciasAsync(CancellationToken cancellationToken = default);
        Task<bool> ExisteProvinciaAsync(string nombre, CancellationToken cancellationToken = default);
        Task AddProvinciaAsync(Provincia provincia, CancellationToken cancellationToken = default);

        Task<Municipio?> GetMunicipioByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Municipio>> GetMunicipiosByProvinciaAsync(int provinciaId, CancellationToken cancellationToken = default);
        Task AddMunicipioAsync(Municipio municipio, CancellationToken cancellationToken = default);

        Task<Sector?> GetSectorByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Sector>> GetSectoresByMunicipioAsync(int municipioId, CancellationToken cancellationToken = default);
        Task AddSectorAsync(Sector sector, CancellationToken cancellationToken = default);

        Task<Direccion?> GetDireccionByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Direccion>> GetDireccionesBySectorAsync(int sectorId, CancellationToken cancellationToken = default);
        Task AddDireccionAsync(Direccion direccion, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
