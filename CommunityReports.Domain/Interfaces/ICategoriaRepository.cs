using CommunityReports.Domain.Entities;

namespace CommunityReports.Domain.Interfaces
{
    public interface ICategoriaRepository
    {
        Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Categoria>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default);
        Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default);
        void Update(Categoria categoria);
        void Remove(Categoria categoria);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
