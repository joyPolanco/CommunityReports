using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Repositories
{
    public class CiudadanoRepository : ICiudadanoRepository
    {
        private readonly ApplicationDbContext _context;

        public CiudadanoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Ciudadano?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Ciudadanos.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        public Task<Ciudadano?> GetByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default) =>
            _context.Ciudadanos.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId, cancellationToken);

        public Task<Ciudadano?> GetByCedulaAsync(string cedula, CancellationToken cancellationToken = default) =>
            _context.Ciudadanos.FirstOrDefaultAsync(c => c.Cedula == cedula, cancellationToken);

        public async Task<IReadOnlyList<Ciudadano>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Ciudadanos.OrderBy(c => c.Apellidos).ToListAsync(cancellationToken);

        public Task<bool> ExisteCedulaAsync(string cedula, CancellationToken cancellationToken = default) =>
            _context.Ciudadanos.AnyAsync(c => c.Cedula == cedula, cancellationToken);

        public async Task AddAsync(Ciudadano ciudadano, CancellationToken cancellationToken = default) =>
            await _context.Ciudadanos.AddAsync(ciudadano, cancellationToken);

        public void Update(Ciudadano ciudadano) => _context.Ciudadanos.Update(ciudadano);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
