using Chavez_Logistica.Dtos.Maestros.Partidas;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chavez_Logistica.Services;

public class PartidaService : IPartidaService
{
    private readonly IPartidaRepository _repo;
    public PartidaService(IPartidaRepository repo) => _repo = repo;

    public async Task<List<PartidaDto>> ListAsync(bool? soloActivas, CancellationToken ct)
        => (await _repo.ListAsync(soloActivas, ct)).Select(Map).ToList();

    public async Task<PartidaDto?> GetByIdAsync(int idPartida, CancellationToken ct)
    {
        var row = await _repo.GetByIdAsync(idPartida, ct);
        return row == null ? null : Map(row);
    }

    public async Task<PartidaCreateResponseDto> CrearAsync(PartidaCreateRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Nombre)) throw new ArgumentException("Nombre es obligatorio.");
        try
        {
            var id = await _repo.CrearAsync(new Partida { Nombre = req.Nombre.Trim(), Activo = true }, ct);
            return new PartidaCreateResponseDto { IdPartida = id };
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            throw new InvalidOperationException("Ya existe una partida con ese nombre.");
        }
    }

    public async Task ActualizarAsync(int idPartida, PartidaUpdateRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Nombre)) throw new ArgumentException("Nombre es obligatorio.");
        try
        {
            await _repo.ActualizarAsync(idPartida, new Partida { Nombre = req.Nombre.Trim(), Activo = req.Activo }, ct);
        }
        catch (SqlException ex) when (ex.Number is 2601 or 2627)
        {
            throw new InvalidOperationException("Ya existe una partida con ese nombre.");
        }
    }

    private static PartidaDto Map(Partida p) => new() { IdPartida = p.IdPartida, Nombre = p.Nombre, Activo = p.Activo };
}
