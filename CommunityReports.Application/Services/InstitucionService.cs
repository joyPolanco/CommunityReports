using CommunityReports.Application.DTOs.Instituciones.Requests;
using CommunityReports.Application.DTOs.Instituciones.Responses;
using CommunityReports.Application.Exceptions;
using CommunityReports.Application.Interfaces;
using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;

namespace CommunityReports.Application.Services
{
    public sealed class InstitucionService : IInstitucionService
    {
        private readonly IInstitucionRepository _repository;

        public InstitucionService(IInstitucionRepository repository)
        {
            _repository = repository;
        }

        public async Task<InstitucionResponseDto> CrearAsync(InstitucionRequestDto request, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExisteNombreAsync(request.Nombre, cancellationToken))
                throw new ConflictAppException("Ya existe una institución con ese nombre.");

            var institucion = new Institucion(request.Nombre, request.Siglas, request.Tipo, request.Telefono, request.Correo, request.SitioWeb);
            await _repository.AddAsync(institucion, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(institucion);
        }

        public async Task<InstitucionResponseDto> ActualizarAsync(int id, InstitucionRequestDto request, CancellationToken cancellationToken = default)
        {
            var institucion = await _repository.GetByIdAsync(id, cancellationToken)
                ?? throw new NotFoundAppException("Institución no encontrada.");

            institucion.ActualizarDatos(request.Nombre, request.Siglas, request.Tipo, request.Telefono, request.Correo, request.SitioWeb);

            _repository.Update(institucion);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(institucion);
        }

        public async Task<IReadOnlyList<InstitucionResponseDto>> ListarAsync(CancellationToken cancellationToken = default) =>
            (await _repository.GetAllAsync(cancellationToken)).Select(Mapear).ToList();

        private static InstitucionResponseDto Mapear(Institucion i) => new()
        {
            Id = i.Id,
            Nombre = i.Nombre,
            Siglas = i.Siglas,
            Tipo = i.Tipo,
            Telefono = i.Telefono,
            Correo = i.Correo,
            SitioWeb = i.SitioWeb
        };
    }
}
