using Chavez_Logistica.Dtos.Maestros.Obras;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class ObraService : IObraService
{
    private readonly IObraRepository _repo;
    public ObraService(IObraRepository repo) => _repo = repo;

    public async Task<List<ObraDto>> ListAsync(bool? soloActivos, CancellationToken ct)
        => (await _repo.ListAsync(soloActivos, ct)).Select(Map).ToList();

    public async Task<ObraDto?> GetByIdAsync(int idObra, CancellationToken ct)
    {
        var row = await _repo.GetByIdAsync(idObra, ct);
        return row == null ? null : Map(row);
    }

    public async Task<ObraCreateResponseDto> CrearAsync(ObraCreateRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Nombre)) throw new ArgumentException("Nombre es obligatorio.");
        var entity = new Obra { Nombre = req.Nombre.Trim(), Ubicacion = string.IsNullOrWhiteSpace(req.Ubicacion) ? null : req.Ubicacion.Trim(), Activa = true };
        var id = await _repo.CrearAsync(entity, ct);
        return new ObraCreateResponseDto { IdObra = id };
    }

    public async Task ActualizarAsync(int idObra, ObraUpdateRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Nombre)) throw new ArgumentException("Nombre es obligatorio.");
        var entity = new Obra { Nombre = req.Nombre.Trim(), Ubicacion = string.IsNullOrWhiteSpace(req.Ubicacion) ? null : req.Ubicacion.Trim(), Activa = req.Activa };
        await _repo.ActualizarAsync(idObra, entity, ct);
    }

    private static ObraDto Map(Obra o) => new() { IdObra=o.IdObra, Nombre=o.Nombre, Ubicacion=o.Ubicacion, Activa = o.Activa };
}
