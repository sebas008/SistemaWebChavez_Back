using Microsoft.AspNetCore.Mvc;
using Chavez_Logistica.Dtos.Logistica.Requerimiento;
using Chavez_Logistica.Dtos.Logistica.Compra;
using Chavez_Logistica.Dtos.Logistica.Atencion;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/logistica")]
public class LogisticaController : ControllerBase
{
    private readonly IRequerimientoService _requerimientos;
    private readonly ICompraService _compras;
    private readonly IAtencionService _atenciones;

    public LogisticaController(
        IRequerimientoService requerimientos,
        ICompraService compras,
        IAtencionService atenciones)
    {
        _requerimientos = requerimientos;
        _compras = compras;
        _atenciones = atenciones;
    }

    [HttpGet("requerimientos")]
    public async Task<ActionResult<List<RequerimientoDto>>> Requerimiento_List(
        [FromQuery] int? idObra,
        [FromQuery] string? estado,
        CancellationToken ct)
        => Ok(await _requerimientos.ListAsync(idObra, estado, ct));

    [HttpGet("requerimientos/{id:int}")]
    public async Task<ActionResult<RequerimientoDto>> Requerimiento_GetById(int id, CancellationToken ct)
        => (await _requerimientos.GetByIdAsync(id, ct)) is { } row ? Ok(row) : NotFound();

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

    [HttpPut("requerimientos/detalle/{idRequerimientoDetalle:int}/destino")]
    public async Task<IActionResult> Requerimiento_Detalle_Destino(
        int idRequerimientoDetalle,
        [FromBody] RequerimientoDetalleDestinoRequestDto req,
        CancellationToken ct)
    {
        await _requerimientos.AsignarDestinoDetalleAsync(idRequerimientoDetalle, req, ct);
        return NoContent();
    }

    [HttpPut("requerimientos/detalle/{idRequerimientoDetalle:int}/estado")]
    public async Task<IActionResult> Requerimiento_Detalle_Estado(
        int idRequerimientoDetalle,
        [FromBody] RequerimientoCambiarEstadoRequestDto req,
        CancellationToken ct)
    {
        await _requerimientos.CambiarEstadoDetalleAsync(idRequerimientoDetalle, req, ct);
        return NoContent();
    }

    [HttpGet("compras-bandeja")]
    public async Task<ActionResult<List<CompraBandejaDto>>> Compras_Bandeja(CancellationToken ct)
        => Ok(await _compras.ListarBandejaAsync(ct));

    [HttpGet("atenciones-bandeja")]
    public async Task<ActionResult<List<AtencionBandejaDto>>> Atenciones_Bandeja(CancellationToken ct)
        => Ok(await _atenciones.ListarBandejaAsync(ct));
}
