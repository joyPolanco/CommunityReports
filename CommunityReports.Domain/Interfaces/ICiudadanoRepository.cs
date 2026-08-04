using CommunityReports.Domain.Entities;

namespace CommunityReports.Domain.Interfaces
{
    /// <summary>Repositorio del perfil de dominio de un Ciudadano (sin nada de autenticación).</summary>
    public interface ICiudadanoRepository
    {
        Task<Ciudadano?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Ciudadano?> GetByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default);
        Task<Ciudadano?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Ciudadano>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<bool> ExisteCedulaAsync(string cedula, CancellationToken cancellationToken = default);

        Task AddAsync(Ciudadano ciudadano, CancellationToken cancellationToken = default);
        void Update(Ciudadano ciudadano);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
