using Chavez_Logistica.Dtos.Maestros.Obras;
using Chavez_Logistica.Dtos.Maestros.Proveedor;
using Chavez_Logistica.Dtos.Maestros.UnidadMedida;
using Chavez_Logistica.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/maestros")]
public class MaestrosController : ControllerBase
{
    private readonly IUnidadMedidaService _unidadMedidaService;
    private readonly IObraService _obraService;
    private readonly IProveedorService _proveedorService;

    public MaestrosController(
     IUnidadMedidaService unidadMedidaService,
     IObraService obraService,
     IProveedorService proveedorService)
    {
        _unidadMedidaService = unidadMedidaService;
        _obraService = obraService;
        _proveedorService = proveedorService;
    }

    // -------- UNIDAD MEDIDA --------
    [HttpGet("unidades-medida")]
    public async Task<ActionResult<List<UnidadMedidaDto>>> UnidadMedida_List(CancellationToken ct)
        => Ok(await _unidadMedidaService.ListAsync(ct));

    [HttpGet("unidades-medida/{id:int}")]
    public async Task<ActionResult<UnidadMedidaDto>> UnidadMedida_GetById(int id, CancellationToken ct)
    {
        var row = await _unidadMedidaService.GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("unidades-medida")]
    public async Task<ActionResult<UnidadMedidaCreateResponseDto>> UnidadMedida_Crear([FromBody] UnidadMedidaCreateRequestDto req, CancellationToken ct)
        => Ok(await _unidadMedidaService.CrearAsync(req, ct));

    [HttpPut("unidades-medida/{id:int}")]
    public async Task<IActionResult> UnidadMedida_Actualizar(int id, [FromBody] UnidadMedidaUpdateRequestDto req, CancellationToken ct)
    {
        await _unidadMedidaService.ActualizarAsync(id, req, ct);
        return NoContent();
    }


    // -------- OBRA --------
    [HttpGet("obras")]
    public async Task<ActionResult<List<ObraDto>>> Obra_List([FromQuery] bool? soloActivas, CancellationToken ct)
        => Ok(await _obraService.ListAsync(soloActivas, ct));

    [HttpGet("obras/{id:int}")]
    public async Task<ActionResult<ObraDto>> Obra_GetById(int id, CancellationToken ct)
    {
        var row = await _obraService.GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("obras")]
    public async Task<ActionResult<ObraCreateResponseDto>> Obra_Crear([FromBody] ObraCreateRequestDto req, CancellationToken ct)
        => Ok(await _obraService.CrearAsync(req, ct));

    [HttpPut("obras/{id:int}")]
    public async Task<IActionResult> Obra_Actualizar(int id, [FromBody] ObraUpdateRequestDto req, CancellationToken ct)
    {
        await _obraService.ActualizarAsync(id, req, ct);
        return NoContent();
    }

    // -------- PROVEEDOR --------
    [HttpGet("proveedores")]
    public async Task<ActionResult<List<ProveedorDto>>> Proveedor_List([FromQuery] bool? soloActivos, CancellationToken ct)
        => Ok(await _proveedorService.ListAsync(soloActivos, ct));

    [HttpGet("proveedores/{id:int}")]
    public async Task<ActionResult<ProveedorDto>> Proveedor_GetById(int id, CancellationToken ct)
    {
        var row = await _proveedorService.GetByIdAsync(id, ct);
        return row == null ? NotFound() : Ok(row);
    }

    [HttpPost("proveedores")]
    public async Task<ActionResult<ProveedorCreateResponseDto>> Proveedor_Crear([FromBody] ProveedorCreateRequestDto req, CancellationToken ct)
        => Ok(await _proveedorService.CrearAsync(req, ct));

    [HttpPut("proveedores/{id:int}")]
    public async Task<IActionResult> Proveedor_Actualizar(int id, [FromBody] ProveedorUpdateRequestDto req, CancellationToken ct)
    {
        await _proveedorService.ActualizarAsync(id, req, ct);
        return NoContent();
    }


}