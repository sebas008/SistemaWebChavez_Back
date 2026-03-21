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
    private readonly IServiceProvider _serviceProvider;
    private readonly ICompraService _compras;
    private readonly IRecepcionCompraService _recepcionesCompra;
    private readonly IRecepcionObraService _recepcionesObra;
    private readonly IAtencionService _atenciones;

    public LogisticaController(
        IRequerimientoService requerimientos,
        IServiceProvider serviceProvider,
        ICompraService compras,
        IRecepcionCompraService recepcionesCompra,
        IRecepcionObraService recepcionesObra,
        IAtencionService atenciones)
    {
        _requerimientos = requerimientos;
        _serviceProvider = serviceProvider;
        _compras = compras;
        _recepcionesCompra = recepcionesCompra;
        _recepcionesObra = recepcionesObra;
        _atenciones = atenciones;
    }

    [HttpGet("requerimientos")]
    public async Task<ActionResult<List<RequerimientoDto>>> Requerimiento_List([FromQuery] int? idObra,[FromQuery] string? estado,CancellationToken ct)
        => Ok(await _requerimientos.ListAsync(idObra, estado, ct));

    [HttpGet("requerimientos/{id:int}")]
    public async Task<ActionResult<RequerimientoDto>> Requerimiento_GetById(int id, CancellationToken ct)
    {
        var row = await _requerimientos.GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("requerimientos")]
    public async Task<ActionResult<RequerimientoCreateResponseDto>> Requerimiento_Crear([FromBody] RequerimientoCreateRequestDto req,CancellationToken ct)
        => Ok(await _requerimientos.CrearAsync(req, ct));

    [HttpPut("requerimientos/{id:int}/estado")]
    public async Task<IActionResult> Requerimiento_CambiarEstado(int id,[FromBody] RequerimientoCambiarEstadoRequestDto req,CancellationToken ct)
    {
        await _requerimientos.CambiarEstadoAsync(id, req, ct);
        return NoContent();
    }

    [HttpPut("requerimientos/detalle/{idRequerimientoDetalle:int}/destino")]
    public async Task<IActionResult> Requerimiento_Detalle_Destino(int idRequerimientoDetalle,[FromBody] RequerimientoDetalleDestinoRequestDto req,CancellationToken ct)
    {
        await _requerimientos.AsignarDestinoDetalleAsync(idRequerimientoDetalle, req, ct);
        return NoContent();
    }

    [HttpPut("requerimientos/detalle/{idRequerimientoDetalle:int}/estado")]
    public async Task<IActionResult> Requerimiento_Detalle_Estado(int idRequerimientoDetalle, [FromBody] RequerimientoDetalleCambiarEstadoRequestDto req, CancellationToken ct)
    {
        await _requerimientos.CambiarEstadoDetalleAsync(idRequerimientoDetalle, req, ct);
        return NoContent();
    }

    [HttpGet("compras-bandeja")]
    public async Task<ActionResult<List<CompraBandejaDto>>> Compra_Bandeja(CancellationToken ct)
        => Ok(await _compras.ListarBandejaAsync(ct));

    [HttpGet("atenciones-bandeja")]
    public async Task<ActionResult<List<AtencionBandejaDto>>> Atencion_Bandeja(CancellationToken ct)
        => Ok(await _atenciones.ListarBandejaAsync(ct));

    private IOrdenFinalService GetOrdenFinalService()
    {
        var service = _serviceProvider.GetService<IOrdenFinalService>();
        if (service is null) throw new InvalidOperationException("No se pudo resolver IOrdenFinalService.");
        return service;
    }

    [HttpGet("ordenes-finales")]
    public async Task<ActionResult<List<OrdenFinalDto>>> OrdenFinal_List([FromQuery] int? idObra, [FromQuery] string? estado, CancellationToken ct)
        => Ok(await GetOrdenFinalService().ListAsync(idObra, estado, ct));

    [HttpGet("ordenes-finales/{id:int}")]
    public async Task<ActionResult<OrdenFinalDto>> OrdenFinal_Get(int id, CancellationToken ct)
        => (await GetOrdenFinalService().GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

    [HttpPost("ordenes-finales")]
    public async Task<ActionResult<OrdenFinalCreateResponseDto>> OrdenFinal_Crear([FromBody] OrdenFinalCreateRequestDto req, CancellationToken ct)
        => Ok(await GetOrdenFinalService().CrearAsync(req, ct));

    [HttpPut("ordenes-finales/{id:int}/estado")]
    public async Task<IActionResult> OrdenFinal_Estado(int id, [FromBody] OrdenFinalCambiarEstadoRequestDto req, CancellationToken ct)
    {
        await GetOrdenFinalService().CambiarEstadoAsync(id, req, ct);
        return NoContent();
    }
}
