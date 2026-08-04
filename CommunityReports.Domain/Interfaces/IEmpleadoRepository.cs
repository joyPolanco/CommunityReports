using CommunityReports.Domain.Entities;

namespace CommunityReports.Domain.Interfaces
{
    /// <summary>Repositorio del perfil de dominio de un Empleado (sin nada de autenticación).</summary>
    public interface IEmpleadoRepository
    {
        Task<Empleado?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Empleado?> GetByUsuarioIdAsync(int usuarioId, CancellationToken cancellationToken = default);
        Task<Empleado?> GetByCodigoAsync(string codigoEmpleado, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Empleado>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<bool> ExisteCodigoEmpleadoAsync(string codigoEmpleado, CancellationToken cancellationToken = default);

        Task AddAsync(Empleado empleado, CancellationToken cancellationToken = default);
        void Update(Empleado empleado);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
