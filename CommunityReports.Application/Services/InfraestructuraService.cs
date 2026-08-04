using CommunityReports.Application.DTOs.Infraestructura.Requests;
using CommunityReports.Application.DTOs.Infraestructura.Responses;
using CommunityReports.Application.Exceptions;
using CommunityReports.Application.Interfaces;
using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Enums;
using CommunityReports.Domain.Interfaces;

namespace CommunityReports.Application.Services
{
    public sealed class InfraestructuraService : IInfraestructuraService
    {
        private readonly IInfraestructuraRepository _repository;

        public InfraestructuraService(IInfraestructuraRepository repository)
        {
            _repository = repository;
        }

        public IReadOnlyList<string> ListarTipos() => Enum.GetNames<TipoInfraestructura>();

        public async Task<InfraestructuraResponseDto> CrearAsync(CreateInfraestructuraRequestDto request, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExisteCodigoAsync(request.Codigo, cancellationToken))
                throw new ConflictAppException("Ya existe una infraestructura con ese código.");

            var infraestructura = new Infraestructura(request.Tipo, request.DireccionId, request.Nombre, request.Codigo, request.Descripcion);
            await _repository.AddAsync(infraestructura, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(infraestructura);
        }

        public async Task<InfraestructuraResponseDto> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var infraestructura = await _repository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundAppException("Infraestructura no encontrada.");

            return Mapear(infraestructura);
        }

        public async Task<IReadOnlyList<InfraestructuraResponseDto>> ListarAsync(CancellationToken cancellationToken = default) =>
            (await _repository.GetAllAsync(cancellationToken)).Select(Mapear).ToList();

        public async Task<IReadOnlyList<InfraestructuraResponseDto>> ListarPorDireccionAsync(int direccionId, CancellationToken cancellationToken = default) =>
            (await _repository.GetByDireccionAsync(direccionId, cancellationToken)).Select(Mapear).ToList();

        public async Task<InfraestructuraResponseDto> ActualizarAsync(int id, UpdateInfraestructuraRequestDto request, CancellationToken cancellationToken = default)
        {
            var infraestructura = await _repository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundAppException("Infraestructura no encontrada.");

            infraestructura.ActualizarDatos(request.Nombre, request.Descripcion);

            _repository.Update(infraestructura);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(infraestructura);
        }

        private static InfraestructuraResponseDto Mapear(Infraestructura i) => new()
        {
            Id = i.Id,
            Tipo = i.Tipo.ToString(),
            DireccionId = i.DireccionId,
            Nombre = i.Nombre,
            Codigo = i.Codigo,
            Descripcion = i.Descripcion
        };
    }
}
