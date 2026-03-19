using Chavez_Logistica.Dtos.Maestros.Partida;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class PartidaService : IPartidaService
{
    private readonly IPartidaRepository _repo;
    public PartidaService(IPartidaRepository repo) => _repo = repo;

    public async Task<List<PartidaDto>> ListAsync(bool? soloActivas, CancellationToken ct)
        => (await _repo.ListAsync(soloActivas, ct)).Select(x => new PartidaDto
        {
            IdPartida = x.IdPartida,
            Nombre = x.Nombre,
            Activo = x.Activo
        }).ToList();

    public async Task<PartidaDto?> GetByIdAsync(int idPartida, CancellationToken ct)
        => (await _repo.GetByIdAsync(idPartida, ct)) is { } x
            ? new PartidaDto { IdPartida = x.IdPartida, Nombre = x.Nombre, Activo = x.Activo }
            : null;

    public async Task<PartidaCreateResponseDto> CrearAsync(PartidaCreateRequestDto req, CancellationToken ct)
    {
        var nombre = (req.Nombre ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre es obligatorio.");
        return new PartidaCreateResponseDto { IdPartida = await _repo.CrearAsync(nombre, ct) };
    }

    public async Task ActualizarAsync(int idPartida, PartidaUpdateRequestDto req, CancellationToken ct)
    {
        var nombre = (req.Nombre ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Nombre es obligatorio.");
        await _repo.ActualizarAsync(idPartida, nombre, req.Activo, ct);
    }
}
