using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;
using CommunityReports.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CommunityReports.Infrastructure.Repositories
{
    public class EmpleadoRepository : IEmpleadoRepository
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<Empleado?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
            _context.Empleados.Include(e => e.Institucion).FirstOrDefaultAsync(e => e.Id == id, cancellationToken);

        public Task<Empleado?> GetByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default) =>
            _context.Empleados.Include(e => e.Institucion).FirstOrDefaultAsync(e => e.UsuarioId == usuarioId, cancellationToken);

        public Task<Empleado?> GetByCodigoAsync(string codigoEmpleado, CancellationToken cancellationToken = default) =>
            _context.Empleados.Include(e => e.Institucion).FirstOrDefaultAsync(e => e.CodigoEmpleado == codigoEmpleado, cancellationToken);

        public async Task<IReadOnlyList<Empleado>> GetAllAsync(CancellationToken cancellationToken = default) =>
            await _context.Empleados.Include(e => e.Institucion).OrderBy(e => e.Cargo).ToListAsync(cancellationToken);

        public Task<bool> ExisteCodigoEmpleadoAsync(string codigoEmpleado, CancellationToken cancellationToken = default) =>
            _context.Empleados.AnyAsync(e => e.CodigoEmpleado == codigoEmpleado, cancellationToken);

        public async Task AddAsync(Empleado empleado, CancellationToken cancellationToken = default) =>
            await _context.Empleados.AddAsync(empleado, cancellationToken);

        public void Update(Empleado empleado) => _context.Empleados.Update(empleado);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
            _context.SaveChangesAsync(cancellationToken);
    }
}
