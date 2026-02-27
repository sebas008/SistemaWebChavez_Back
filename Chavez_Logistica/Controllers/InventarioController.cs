using Microsoft.AspNetCore.Mvc;
using Chavez_Logistica.Dtos.Inventario.Almacen;
using Chavez_Logistica.Dtos.Inventario.Item;
using Chavez_Logistica.Dtos.Inventario.Stock;
using Chavez_Logistica.Dtos.Inventario.Kardex;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/inventario")]
public class InventarioController : ControllerBase
{
    private readonly IInventarioService _service;
    public InventarioController(IInventarioService service) => _service = service;

    // ---------------- ALMACEN ----------------
    [HttpGet("almacenes")]
    public async Task<ActionResult<List<AlmacenDto>>> Almacen_List([FromQuery] bool? soloActivos, CancellationToken ct)
        => Ok(await _service.Almacen_ListAsync(soloActivos, ct));

    [HttpGet("almacenes/{id:int}")]
    public async Task<ActionResult<AlmacenDto>> Almacen_GetById(int id, CancellationToken ct)
    {
        var row = await _service.Almacen_GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("almacenes")]
    public async Task<ActionResult<AlmacenCreateResponseDto>> Almacen_Crear([FromBody] AlmacenCreateRequestDto req, CancellationToken ct)
        => Ok(await _service.Almacen_CrearAsync(req, ct));

    [HttpPut("almacenes/{id:int}")]
    public async Task<IActionResult> Almacen_Actualizar(int id, [FromBody] AlmacenUpdateRequestDto req, CancellationToken ct)
    {
        await _service.Almacen_ActualizarAsync(id, req, ct);
        return NoContent();
    }

    // ---------------- ITEM ----------------
    [HttpGet("items")]
    public async Task<ActionResult<List<ItemDto>>> Item_List([FromQuery] bool? soloActivos, CancellationToken ct)
        => Ok(await _service.Item_ListAsync(soloActivos, ct));

    [HttpGet("items/{id:int}")]
    public async Task<ActionResult<ItemDto>> Item_GetById(int id, CancellationToken ct)
    {
        var row = await _service.Item_GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("items")]
    public async Task<ActionResult<ItemCreateResponseDto>> Item_Crear([FromBody] ItemCreateRequestDto req, CancellationToken ct)
        => Ok(await _service.Item_CrearAsync(req, ct));

    [HttpPut("items/{id:int}")]
    public async Task<IActionResult> Item_Actualizar(int id, [FromBody] ItemUpdateRequestDto req, CancellationToken ct)
    {
        await _service.Item_ActualizarAsync(id, req, ct);
        return NoContent();
    }

    // ---------------- STOCK ----------------
    [HttpGet("stock")]
    public async Task<ActionResult<List<StockDto>>> Stock_List([FromQuery] int? idAlmacen, CancellationToken ct)
        => Ok(await _service.Stock_ListAsync(idAlmacen, ct));

    // ---------------- KARDEX ----------------
    [HttpGet("kardex")]
    public async Task<ActionResult<List<KardexDto>>> Kardex_List(
        [FromQuery] int? idAlmacen,
        [FromQuery] int? idItem,
        [FromQuery] DateTime? desde,
        [FromQuery] DateTime? hasta,
        CancellationToken ct)
        => Ok(await _service.Kardex_ListAsync(idAlmacen, idItem, desde, hasta, ct));
}