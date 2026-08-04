using CommunityReports.Domain.Entities;

namespace CommunityReports.Domain.Interfaces
{
    public interface IInstitucionRepository
    {
        Task<Institucion?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Institucion>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task AddAsync(Institucion institucion, CancellationToken cancellationToken = default);
        void Update(Institucion institucion);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
