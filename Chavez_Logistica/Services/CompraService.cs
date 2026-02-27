using Chavez_Logistica.Dtos.Logistica.Compra;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class CompraService : ICompraService
{
    private readonly ICompraRepository _repo;
    public CompraService(ICompraRepository repo) => _repo = repo;

    public async Task<List<CompraDto>> ListAsync(int? idProveedor, int? idObra, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idProveedor, idObra, estado, ct)).Select(c => new CompraDto {
            IdCompra=c.IdCompra, Codigo=c.Codigo, Fecha=c.Fecha, IdProveedor=c.IdProveedor, IdObra=c.IdObra, Estado=c.Estado, Observacion=c.Observacion
        }).ToList();

    public async Task<CompraDto?> GetByIdAsync(int idCompra, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idCompra, ct);
        if (h == null) return null;
        var det = await _repo.Detalle_ListByCompraAsync(idCompra, ct);
        return new CompraDto{
            IdCompra=h.IdCompra, Codigo=h.Codigo, Fecha=h.Fecha, IdProveedor=h.IdProveedor, IdObra=h.IdObra, Estado=h.Estado, Observacion=h.Observacion,
            Detalle=det.Select(d=> new CompraDetalleDto{ IdCompraDetalle=d.IdCompraDetalle, IdItem=d.IdItem, Cantidad=d.Cantidad, PrecioUnitario=d.PrecioUnitario, Observacion=d.Observacion }).ToList()
        };
    }

    public async Task<CompraCreateResponseDto> CrearAsync(CompraCreateRequestDto req, CancellationToken ct)
    {
        if (req.IdProveedor<=0) throw new ArgumentException("IdProveedor inválido.");
        if (req.IdObra<=0) throw new ArgumentException("IdObra inválido.");
        if (req.Detalle==null || req.Detalle.Count==0) throw new ArgumentException("Detalle es obligatorio.");
        var det = req.Detalle.Select(d=> new CompraDetalle{ IdItem=d.IdItem, Cantidad=d.Cantidad, PrecioUnitario=d.PrecioUnitario, Observacion=string.IsNullOrWhiteSpace(d.Observacion)?null:d.Observacion.Trim() });
        var (id,cod)= await _repo.CrearAsync(req.IdProveedor, req.IdObra, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), req.IdUsuario, det, ct);
        return new CompraCreateResponseDto{ IdCompra=id, Codigo=cod };
    }

    public async Task CambiarEstadoAsync(int idCompra, CompraCambiarEstadoRequestDto req, CancellationToken ct)
        => await _repo.CambiarEstadoAsync(idCompra, req.Estado.Trim().ToUpperInvariant(), req.IdUsuario, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), ct);
}
