using Chavez_Logistica.Dtos.Maestros.UnidadMedida;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class UnidadMedidaService : IUnidadMedidaService
{
    private readonly IUnidadMedidaRepository _repo;
    public UnidadMedidaService(IUnidadMedidaRepository repo) => _repo = repo;

    public async Task<List<UnidadMedidaDto>> ListAsync(CancellationToken ct)
    {
        var rows = await _repo.ListAsync(ct);
        return rows.Select(Map).ToList();
    }

    public async Task<UnidadMedidaDto?> GetByIdAsync(int idUnidadMedida, CancellationToken ct)
    {
        var row = await _repo.GetByIdAsync(idUnidadMedida, ct);
        return row == null ? null : Map(row);
    }

    public async Task<UnidadMedidaCreateResponseDto> CrearAsync(UnidadMedidaCreateRequestDto req, CancellationToken ct)
    {
        if (req == null) throw new ArgumentNullException(nameof(req));
        if (string.IsNullOrWhiteSpace(req.Codigo)) throw new ArgumentException("Codigo es obligatorio.");
        if (string.IsNullOrWhiteSpace(req.Nombre)) throw new ArgumentException("Nombre es obligatorio.");

        var entity = new UnidadMedida
        {
            Codigo = req.Codigo.Trim().ToUpperInvariant(),
            Nombre = req.Nombre.Trim()
        };

        var id = await _repo.CrearAsync(entity, ct);
        return new UnidadMedidaCreateResponseDto { IdUnidadMedida = id };
    }

    public async Task ActualizarAsync(int idUnidadMedida, UnidadMedidaUpdateRequestDto req, CancellationToken ct)
    {
        if (req == null) throw new ArgumentNullException(nameof(req));
        if (string.IsNullOrWhiteSpace(req.Nombre)) throw new ArgumentException("Nombre es obligatorio.");

        await _repo.ActualizarAsync(idUnidadMedida, req.Nombre.Trim(), ct);
    }

    private static UnidadMedidaDto Map(UnidadMedida u) => new()
    {
        IdUnidadMedida = u.IdUnidadMedida,
        Codigo = u.Codigo,
        Nombre = u.Nombre
    };
}