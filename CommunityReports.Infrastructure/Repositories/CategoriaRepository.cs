using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoriaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Categoria?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Set<Categoria>().FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Categoria>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Set<Categoria>().OrderBy(c => c.Nombre).ToListAsync(cancellationToken);

        public Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default) =>
            _context.Set<Categoria>().AnyAsync(c => c.Nombre == nombre.Trim(), cancellationToken);

        public async Task AddAsync(Categoria categoria, CancellationToken cancellationToken = default) =>
            await _context.Set<Categoria>().AddAsync(categoria, cancellationToken);

        public void Update(Categoria categoria) => _context.Set<Categoria>().Update(categoria);

        public void Remove(Categoria categoria) => _context.Set<Categoria>().Remove(categoria);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
