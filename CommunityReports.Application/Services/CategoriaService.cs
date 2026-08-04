using CommunityReports.Application.DTOs.Categorias.Requests;
using CommunityReports.Application.DTOs.Categorias.Responses;
using CommunityReports.Application.Exceptions;
using CommunityReports.Application.Interfaces;
using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;

namespace CommunityReports.Application.Services
{
    public sealed class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _repository;

        public CategoriaService(ICategoriaRepository repository)
        {
            _repository = repository;
        }

        public async Task<CategoriaResponseDto> CrearAsync(CategoriaRequestDto request, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExisteNombreAsync(request.Nombre, cancellationToken))
                throw new ConflictAppException("Ya existe una categoría con ese nombre.");

            var categoria = new Categoria(request.Nombre, request.Color, request.TiempoRespuesta);
            await _repository.AddAsync(categoria, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(categoria);
        }

        public async Task<CategoriaResponseDto> ActualizarAsync(int id, CategoriaRequestDto request, CancellationToken cancellationToken = default)
        {
            var categoria = await _repository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundAppException("Categoría no encontrada.");

            categoria.ActualizarDatos(request.Nombre, request.Color, request.TiempoRespuesta);

            _repository.Update(categoria);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(categoria);
        }

        public async Task<IReadOnlyList<CategoriaResponseDto>> ListarAsync(CancellationToken cancellationToken = default) =>
            (await _repository.GetAllAsync(cancellationToken)).Select(Mapear).ToList();

        public async Task EliminarAsync(int id, CancellationToken cancellationToken = default)
        {
            var categoria = await _repository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundAppException("Categoría no encontrada.");

            _repository.Remove(categoria);
            await _repository.SaveChangesAsync(cancellationToken);
        }

        private static CategoriaResponseDto Mapear(Categoria c) => new()
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Color = c.Color,
            TiempoRespuesta = c.TiempoRespuesta
        };
    }
}
