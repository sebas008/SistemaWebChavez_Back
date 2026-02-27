using Microsoft.AspNetCore.Mvc;
using Chavez_Logistica.Dtos.Logistica.Requerimiento;
using Chavez_Logistica.Interfaces;
using Chavez_Logistica.Dtos.Logistica.RecepcionCompra;
using Chavez_Logistica.Dtos.Logistica.Atencion;
using Chavez_Logistica.Dtos.Logistica.RecepcionObra;
using Chavez_Logistica.Dtos.Logistica.Compra;
using Chavez_Logistica.Dtos.Logistica.OrdenFinal;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/logistica")]
public class LogisticaController : ControllerBase
{
    private readonly IRequerimientoService _requerimientos;
    private readonly IOrdenFinalService _ordenesFinales;
    private readonly ICompraService _compras;
    private readonly IRecepcionCompraService _recepcionesCompra;
    private readonly IRecepcionObraService _recepcionesObra;
    private readonly IAtencionService _atenciones;

    public LogisticaController(
        IRequerimientoService requerimientos,
        IOrdenFinalService ordenesFinales,
        ICompraService compras,
        IRecepcionCompraService recepcionesCompra,
        IRecepcionObraService recepcionesObra,
        IAtencionService atenciones)
    {
        _requerimientos = requerimientos;
        _ordenesFinales = ordenesFinales;
        _compras = compras;
        _recepcionesCompra = recepcionesCompra;
        _recepcionesObra = recepcionesObra;
        _atenciones = atenciones;
    }


    // -------- REQUERIMIENTO --------
    [HttpGet("requerimientos")]
    public async Task<ActionResult<List<RequerimientoDto>>> Requerimiento_List(
        [FromQuery] int? idObra,
        [FromQuery] string? estado,
        CancellationToken ct)
        => Ok(await _requerimientos.ListAsync(idObra, estado, ct));

    [HttpGet("requerimientos/{id:int}")]
    public async Task<ActionResult<RequerimientoDto>> Requerimiento_GetById(int id, CancellationToken ct)
    {
        var row = await _requerimientos.GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("requerimientos")]
    public async Task<ActionResult<RequerimientoCreateResponseDto>> Requerimiento_Crear(
        [FromBody] RequerimientoCreateRequestDto req,
        CancellationToken ct)
        => Ok(await _requerimientos.CrearAsync(req, ct));

    [HttpPut("requerimientos/{id:int}/estado")]
    public async Task<IActionResult> Requerimiento_CambiarEstado(
        int id,
        [FromBody] RequerimientoCambiarEstadoRequestDto req,
        CancellationToken ct)
    {
        await _requerimientos.CambiarEstadoAsync(id, req, ct);
        return NoContent();
    }
    // ===== ORDENES FINALES =====
    [HttpGet("ordenes-finales")]
    public async Task<ActionResult<List<OrdenFinalDto>>> OrdenFinal_List([FromQuery] int? idObra, [FromQuery] string? estado, CancellationToken ct)
        => Ok(await _ordenesFinales.ListAsync(idObra, estado, ct));

    [HttpGet("ordenes-finales/{id:int}")]
    public async Task<ActionResult<OrdenFinalDto>> OrdenFinal_Get(int id, CancellationToken ct)
        => (await _ordenesFinales.GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

    [HttpPost("ordenes-finales")]
    public async Task<ActionResult<OrdenFinalCreateResponseDto>> OrdenFinal_Crear([FromBody] OrdenFinalCreateRequestDto req, CancellationToken ct)
        => Ok(await _ordenesFinales.CrearAsync(req, ct));

    [HttpPut("ordenes-finales/{id:int}/estado")]
    public async Task<IActionResult> OrdenFinal_Estado(int id, [FromBody] OrdenFinalCambiarEstadoRequestDto req, CancellationToken ct)
    {
        await _ordenesFinales.CambiarEstadoAsync(id, req, ct);
        return NoContent();
    }

    // ===== COMPRAS =====
    [HttpGet("compras")]
    public async Task<ActionResult<List<CompraDto>>> Compras_List([FromQuery] int? idProveedor, [FromQuery] int? idObra, [FromQuery] string? estado, CancellationToken ct)
        => Ok(await _compras.ListAsync(idProveedor, idObra, estado, ct));

    [HttpGet("compras/{id:int}")]
    public async Task<ActionResult<CompraDto>> Compras_Get(int id, CancellationToken ct)
        => (await _compras.GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

    [HttpPost("compras")]
    public async Task<ActionResult<CompraCreateResponseDto>> Compras_Crear([FromBody] CompraCreateRequestDto req, CancellationToken ct)
        => Ok(await _compras.CrearAsync(req, ct));

    [HttpPut("compras/{id:int}/estado")]
    public async Task<IActionResult> Compras_Estado(int id, [FromBody] CompraCambiarEstadoRequestDto req, CancellationToken ct)
    {
        await _compras.CambiarEstadoAsync(id, req, ct);
        return NoContent();
    }

    // ===== RECEPCION COMPRA =====
    [HttpGet("recepciones-compra")]
    public async Task<ActionResult<List<RecepcionCompraDto>>> RecepcionCompra_List([FromQuery] int? idCompra, [FromQuery] string? estado, CancellationToken ct)
        => Ok(await _recepcionesCompra.ListAsync(idCompra, estado, ct));

    [HttpGet("recepciones-compra/{id:int}")]
    public async Task<ActionResult<RecepcionCompraDto>> RecepcionCompra_Get(int id, CancellationToken ct)
        => (await _recepcionesCompra.GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

    [HttpPost("recepciones-compra")]
    public async Task<ActionResult<RecepcionCompraRegistrarResponseDto>> RecepcionCompra_Registrar([FromBody] RecepcionCompraRegistrarRequestDto req, CancellationToken ct)
        => Ok(await _recepcionesCompra.RegistrarAsync(req, ct));

    // ===== RECEPCION OBRA =====
    [HttpGet("recepciones-obra")]
    public async Task<ActionResult<List<RecepcionObraDto>>> RecepcionObra_List([FromQuery] int? idOrdenFinal, [FromQuery] string? estado, CancellationToken ct)
        => Ok(await _recepcionesObra.ListAsync(idOrdenFinal, estado, ct));

    [HttpGet("recepciones-obra/{id:int}")]
    public async Task<ActionResult<RecepcionObraDto>> RecepcionObra_Get(int id, CancellationToken ct)
        => (await _recepcionesObra.GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

    [HttpPost("recepciones-obra")]
    public async Task<ActionResult<RecepcionObraRegistrarResponseDto>> RecepcionObra_Registrar([FromBody] RecepcionObraRegistrarRequestDto req, CancellationToken ct)
        => Ok(await _recepcionesObra.RegistrarAsync(req, ct));

    // ===== ATENCION =====
    [HttpGet("atenciones")]
    public async Task<ActionResult<List<AtencionDto>>> Atencion_List([FromQuery] int? idObra, [FromQuery] int? idAlmacenOrigen, [FromQuery] int? idAlmacenDestino, [FromQuery] string? estado, CancellationToken ct)
        => Ok(await _atenciones.ListAsync(idObra, idAlmacenOrigen, idAlmacenDestino, estado, ct));

    [HttpGet("atenciones/{id:int}")]
    public async Task<ActionResult<AtencionDto>> Atencion_Get(int id, CancellationToken ct)
        => (await _atenciones.GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

    [HttpPost("atenciones/desde-almacen-interno")]
    public async Task<ActionResult<AtencionRegistrarResponseDto>> Atencion_Registrar([FromBody] AtencionRegistrarRequestDto req, CancellationToken ct)
        => Ok(await _atenciones.RegistrarDesdeAlmacenInternoAsync(req, ct));
}
