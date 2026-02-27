using Chavez_Logistica.Dtos.Inventario.Almacen;
using Chavez_Logistica.Dtos.Inventario.Item;
using Chavez_Logistica.Dtos.Inventario.Kardex;
using Chavez_Logistica.Dtos.Inventario.Stock;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class InventarioService : IInventarioService
{
    private readonly IInventarioRepository _repo;

    public InventarioService(IInventarioRepository repo)
    {
        _repo = repo;
    }

    // ---------------- ALMACEN ----------------
    public async Task<List<AlmacenDto>> Almacen_ListAsync(bool? soloActivos, CancellationToken ct)
    {
        var rows = await _repo.Almacen_ListAsync(soloActivos, ct);
        return rows.Select(Map).ToList();
    }

    public async Task<AlmacenDto?> Almacen_GetByIdAsync(int idAlmacen, CancellationToken ct)
    {
        var row = await _repo.Almacen_GetByIdAsync(idAlmacen, ct);
        return row == null ? null : Map(row);
    }

    public async Task<AlmacenCreateResponseDto> Almacen_CrearAsync(AlmacenCreateRequestDto req, CancellationToken ct)
    {
        if (req == null) throw new ArgumentNullException(nameof(req));

        var tipo = (req.Tipo ?? "").Trim().ToUpperInvariant();
        if (tipo != "INTERNO" && tipo != "OBRA")
            throw new ArgumentException("Tipo debe ser INTERNO u OBRA.");

        if (tipo == "OBRA" && req.IdObra is null)
            throw new ArgumentException("IdObra es obligatorio cuando Tipo=OBRA.");

        if (string.IsNullOrWhiteSpace(req.Codigo))
            throw new ArgumentException("Codigo es obligatorio.");

        if (string.IsNullOrWhiteSpace(req.Nombre))
            throw new ArgumentException("Nombre es obligatorio.");

        var entity = new Almacen
        {
            Tipo = tipo,
            IdObra = req.IdObra,
            Codigo = req.Codigo.Trim().ToUpperInvariant(),
            Nombre = req.Nombre.Trim(),
            Activo = true
        };

        var id = await _repo.Almacen_CrearAsync(entity, ct);
        return new AlmacenCreateResponseDto { IdAlmacen = id };
    }

    public async Task Almacen_ActualizarAsync(int idAlmacen, AlmacenUpdateRequestDto req, CancellationToken ct)
    {
        if (req == null) throw new ArgumentNullException(nameof(req));

        if (string.IsNullOrWhiteSpace(req.Nombre))
            throw new ArgumentException("Nombre es obligatorio.");

        await _repo.Almacen_ActualizarAsync(idAlmacen, req.Nombre.Trim(), req.Activo, ct);
    }

    // ---------------- ITEM ----------------
    public async Task<List<ItemDto>> Item_ListAsync(bool? soloActivos, CancellationToken ct)
    {
        var rows = await _repo.Item_ListAsync(soloActivos, ct);
        return rows.Select(Map).ToList();
    }

    public async Task<ItemDto?> Item_GetByIdAsync(int idItem, CancellationToken ct)
    {
        var row = await _repo.Item_GetByIdAsync(idItem, ct);
        return row == null ? null : Map(row);
    }

    public async Task<ItemCreateResponseDto> Item_CrearAsync(ItemCreateRequestDto req, CancellationToken ct)
    {
        if (req == null) throw new ArgumentNullException(nameof(req));

        if (string.IsNullOrWhiteSpace(req.Descripcion))
            throw new ArgumentException("Descripcion es obligatoria.");

        var entity = new Item
        {
            Partida = string.IsNullOrWhiteSpace(req.Partida) ? null : req.Partida.Trim(),
            Descripcion = req.Descripcion.Trim(),
            IdUnidadMedida = req.IdUnidadMedida,
            Activo = true
        };

        var id = await _repo.Item_CrearAsync(entity, ct);
        return new ItemCreateResponseDto { IdItem = id };
    }

    public async Task Item_ActualizarAsync(int idItem, ItemUpdateRequestDto req, CancellationToken ct)
    {
        if (req == null) throw new ArgumentNullException(nameof(req));

        if (string.IsNullOrWhiteSpace(req.Descripcion))
            throw new ArgumentException("Descripcion es obligatoria.");

        var entity = new Item
        {
            Partida = string.IsNullOrWhiteSpace(req.Partida) ? null : req.Partida.Trim(),
            Descripcion = req.Descripcion.Trim(),
            IdUnidadMedida = req.IdUnidadMedida,
            Activo = req.Activo
        };

        await _repo.Item_ActualizarAsync(idItem, entity, ct);
    }

    // ---------------- STOCK ----------------
    public async Task<List<StockDto>> Stock_ListAsync(int? idAlmacen, CancellationToken ct)
    {
        var rows = await _repo.Stock_ListAsync(idAlmacen, ct);
        return rows.Select(s => new StockDto
        {
            IdAlmacen = s.IdAlmacen,
            IdItem = s.IdItem,
            StockActual = s.StockActual
        }).ToList();
    }

    // ---------------- KARDEX ----------------
    public async Task<List<KardexDto>> Kardex_ListAsync(int? idAlmacen, int? idItem, DateTime? desde, DateTime? hasta, CancellationToken ct)
    {
        var rows = await _repo.Kardex_ListAsync(idAlmacen, idItem, desde, hasta, ct);
        return rows.Select(k => new KardexDto
        {
            IdKardex = k.IdKardex,
            Fecha = k.Fecha,
            IdAlmacen = k.IdAlmacen,
            IdItem = k.IdItem,
            TipoMov = k.TipoMov,
            Cantidad = k.Cantidad,
            Referencia = k.Referencia,
            Observacion = k.Observacion,
            IdUsuario = k.IdUsuario
        }).ToList();
    }

    // ---------------- MAPS ----------------
    private static AlmacenDto Map(Almacen a) => new()
    {
        IdAlmacen = a.IdAlmacen,
        Tipo = a.Tipo,
        IdObra = a.IdObra,
        Codigo = a.Codigo,
        Nombre = a.Nombre,
        Activo = a.Activo
    };

    private static ItemDto Map(Item i) => new()
    {
        IdItem = i.IdItem,
        Partida = i.Partida,
        Descripcion = i.Descripcion,
        IdUnidadMedida = i.IdUnidadMedida,
        Activo = i.Activo
    };
}