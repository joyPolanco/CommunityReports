using CommunityReports.Application.DTOs.Ubicacion.Requests;
using CommunityReports.Application.DTOs.Ubicacion.Responses;
using CommunityReports.Application.Exceptions;
using CommunityReports.Application.Interfaces;
using CommunityReports.Domain.Entities;
using CommunityReports.Domain.Interfaces;

namespace CommunityReports.Application.Services
{
    public sealed class UbicacionService : IUbicacionService
    {
        private readonly IUbicacionRepository _repository;

        public UbicacionService(IUbicacionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ProvinciaResponseDto> CrearProvinciaAsync(CreateProvinciaRequestDto request, CancellationToken cancellationToken = default)
        {
            if (await _repository.ExisteProvinciaAsync(request.Nombre, cancellationToken))
                throw new ConflictAppException("Ya existe una provincia con ese nombre.");

            var provincia = new Provincia(request.Nombre);
            await _repository.AddProvinciaAsync(provincia, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(provincia);
        }

        public async Task<IReadOnlyList<ProvinciaResponseDto>> ListarProvinciasAsync(CancellationToken cancellationToken = default) =>
            (await _repository.GetProvinciasAsync(cancellationToken)).Select(Mapear).ToList();

        public async Task<MunicipioResponseDto> CrearMunicipioAsync(CreateMunicipioRequestDto request, CancellationToken cancellationToken = default)
        {
            _ = await _repository.GetProvinciaByIdAsync(request.ProvinciaId, cancellationToken)
                ?? throw new NotFoundAppException("La provincia indicada no existe.");

            var municipio = new Municipio(request.Nombre, request.ProvinciaId);
            await _repository.AddMunicipioAsync(municipio, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(municipio);
        }

        public async Task<IReadOnlyList<MunicipioResponseDto>> ListarMunicipiosPorProvinciaAsync(int provinciaId, CancellationToken cancellationToken = default) =>
            (await _repository.GetMunicipiosByProvinciaAsync(provinciaId, cancellationToken)).Select(Mapear).ToList();

        public async Task<SectorResponseDto> CrearSectorAsync(CreateSectorRequestDto request, CancellationToken cancellationToken = default)
        {
            _ = await _repository.GetMunicipioByIdAsync(request.MunicipioId, cancellationToken)
                ?? throw new NotFoundAppException("El municipio indicado no existe.");

            var sector = new Sector(request.Nombre, request.MunicipioId);
            await _repository.AddSectorAsync(sector, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(sector);
        }

        public async Task<IReadOnlyList<SectorResponseDto>> ListarSectoresPorMunicipioAsync(int municipioId, CancellationToken cancellationToken = default) =>
            (await _repository.GetSectoresByMunicipioAsync(municipioId, cancellationToken)).Select(Mapear).ToList();

        public async Task<DireccionResponseDto> CrearDireccionAsync(CreateDireccionRequestDto request, CancellationToken cancellationToken = default)
        {
            _ = await _repository.GetSectorByIdAsync(request.SectorId, cancellationToken)
                ?? throw new NotFoundAppException("El sector indicado no existe.");

            var direccion = new Direccion(request.SectorId, request.Calle, request.Referencia, request.CodigoPostal, request.Latitud, request.Longitud);
            await _repository.AddDireccionAsync(direccion, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return Mapear(direccion);
        }

        public async Task<IReadOnlyList<DireccionResponseDto>> ListarDireccionesPorSectorAsync(int sectorId, CancellationToken cancellationToken = default) =>
            (await _repository.GetDireccionesBySectorAsync(sectorId, cancellationToken)).Select(Mapear).ToList();

        private static ProvinciaResponseDto Mapear(Provincia p) => new() { Id = p.Id, Nombre = p.Nombre };

        private static MunicipioResponseDto Mapear(Municipio m) => new() { Id = m.Id, Nombre = m.Nombre, ProvinciaId = m.ProvinciaId };

        private static SectorResponseDto Mapear(Sector s) => new() { Id = s.Id, Nombre = s.Nombre, MunicipioId = s.MunicipioId };

        private static DireccionResponseDto Mapear(Direccion d) => new()
        {
            Id = d.Id,
            SectorId = d.SectorId,
            Calle = d.Calle,
            Referencia = d.Referencia,
            CodigoPostal = d.CodigoPostal,
            Latitud = d.Latitud,
            Longitud = d.Longitud
        };
    }
}
