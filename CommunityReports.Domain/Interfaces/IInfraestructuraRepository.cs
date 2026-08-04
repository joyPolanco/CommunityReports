using CommunityReports.Domain.Entities;

namespace CommunityReports.Domain.Interfaces
{
    /// <summary>
    /// Repositorio de Infraestructura. Ya no expone métodos para "TipoInfraestructura"
    /// porque esa clasificación pasó a ser un enum (ver Domain.Enums.TipoInfraestructura);
    /// listar los tipos disponibles no requiere ir a la base de datos.
    /// </summary>
    public interface IInfraestructuraRepository
    {
        Task<Infraestructura?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Infraestructura?> GetByCodigoAsync(string codigo, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Infraestructura>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Infraestructura>> GetByDireccionAsync(int direccionId, CancellationToken cancellationToken = default);
        Task<bool> ExisteCodigoAsync(string codigo, CancellationToken cancellationToken = default);
        Task AddAsync(Infraestructura infraestructura, CancellationToken cancellationToken = default);
        void Update(Infraestructura infraestructura);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
