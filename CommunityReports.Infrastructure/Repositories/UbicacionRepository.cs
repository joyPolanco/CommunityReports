using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Repositories
{
    public class UbicacionRepository : IUbicacionRepository
    {
        private readonly ApplicationDbContext _context;

        public UbicacionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Provincia?> GetProvinciaByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Set<Provincia>().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Provincia>> GetProvinciasAsync(CancellationToken cancellationToken = default) =>
            await _context.Set<Provincia>().OrderBy(p => p.Nombre).ToListAsync(cancellationToken);

        public Task<bool> ExisteProvinciaAsync(string nombre, CancellationToken cancellationToken = default) =>
            _context.Set<Provincia>().AnyAsync(p => p.Nombre == nombre.Trim(), cancellationToken);

        public async Task AddProvinciaAsync(Provincia provincia, CancellationToken cancellationToken = default) =>
            await _context.Set<Provincia>().AddAsync(provincia, cancellationToken);

        public Task<Municipio?> GetMunicipioByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Set<Municipio>().FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Municipio>> GetMunicipiosByProvinciaAsync(int provinciaId, CancellationToken cancellationToken = default) =>
            await _context.Set<Municipio>().Where(m => m.ProvinciaId == provinciaId).OrderBy(m => m.Nombre).ToListAsync(cancellationToken);

        public async Task AddMunicipioAsync(Municipio municipio, CancellationToken cancellationToken = default) =>
            await _context.Set<Municipio>().AddAsync(municipio, cancellationToken);

        public Task<Sector?> GetSectorByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Set<Sector>().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Sector>> GetSectoresByMunicipioAsync(int municipioId, CancellationToken cancellationToken = default) =>
            await _context.Set<Sector>().Where(s => s.MunicipioId == municipioId).OrderBy(s => s.Nombre).ToListAsync(cancellationToken);

        public async Task AddSectorAsync(Sector sector, CancellationToken cancellationToken = default) =>
            await _context.Set<Sector>().AddAsync(sector, cancellationToken);

        public Task<Direccion?> GetDireccionByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Set<Direccion>().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        public async Task<IReadOnlyList<Direccion>> GetDireccionesBySectorAsync(int sectorId, CancellationToken cancellationToken = default) =>
            await _context.Set<Direccion>().Where(d => d.SectorId == sectorId).ToListAsync(cancellationToken);

        public async Task AddDireccionAsync(Direccion direccion, CancellationToken cancellationToken = default) =>
            await _context.Set<Direccion>().AddAsync(direccion, cancellationToken);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
