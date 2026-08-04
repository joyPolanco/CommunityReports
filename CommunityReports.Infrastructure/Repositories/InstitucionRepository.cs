using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Repositories
{
    public class InstitucionRepository : IInstitucionRepository
    {
        private readonly ApplicationDbContext _context;

        public InstitucionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Institucion?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Instituciones.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Institucion>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Instituciones.OrderBy(i => i.Nombre).ToListAsync(cancellationToken);

        public Task<bool> ExisteNombreAsync(string nombre, CancellationToken cancellationToken = default) =>
            _context.Instituciones.AnyAsync(i => i.Nombre == nombre.Trim(), cancellationToken);

        public async Task AddAsync(Institucion institucion, CancellationToken cancellationToken = default) =>
            await _context.Instituciones.AddAsync(institucion, cancellationToken);

        public void Update(Institucion institucion) => _context.Instituciones.Update(institucion);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
