using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Repositories
{
    public class InfraestructuraRepository : IInfraestructuraRepository
    {
        private readonly ApplicationDbContext _context;

        public InfraestructuraRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Infraestructura?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Infraestructuras.FirstOrDefaultAsync(i => i.Id == id, cancellationToken);

        public Task<Infraestructura?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default) =>
            _context.Infraestructuras.FirstOrDefaultAsync(i => i.Codigo == codigo, cancellationToken);

        public async Task<IReadOnlyList<Infraestructura>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Infraestructuras.OrderBy(i => i.Nombre).ToListAsync(cancellationToken);

        public async Task<IReadOnlyList<Infraestructura>> GetByDireccionAsync(int direccionId, CancellationToken cancellationToken = default) =>
            await _context.Infraestructuras.Where(i => i.DireccionId == direccionId).ToListAsync(cancellationToken);

        public Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default) =>
            _context.Infraestructuras.AnyAsync(i => i.Codigo == codigo, cancellationToken);

        public async Task AddAsync(Infraestructura infraestructura, CancellationToken cancellationToken = default) =>
            await _context.Infraestructuras.AddAsync(infraestructura, cancellationToken);

        public void Update(Infraestructura infraestructura) => _context.Infraestructuras.Update(infraestructura);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
