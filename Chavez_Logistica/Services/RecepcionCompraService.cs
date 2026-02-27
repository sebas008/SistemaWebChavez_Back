using Chavez_Logistica.Dtos.Logistica.RecepcionCompra;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class RecepcionCompraService : IRecepcionCompraService
{
    private readonly IRecepcionCompraRepository _repo;
    public RecepcionCompraService(IRecepcionCompraRepository repo) => _repo = repo;

    public async Task<List<RecepcionCompraDto>> ListAsync(int? idCompra, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idCompra, estado, ct)).Select(r => new RecepcionCompraDto{
            IdRecepcionCompra=r.IdRecepcionCompra, Codigo=r.Codigo, IdCompra=r.IdCompra, IdAlmacenDestino=r.IdAlmacenDestino, FechaRecepcion=r.FechaRecepcion, Estado=r.Estado, Observacion=r.Observacion
        }).ToList();

    public async Task<RecepcionCompraDto?> GetByIdAsync(int idRecepcionCompra, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idRecepcionCompra, ct);
        if (h == null) return null;
        var det = await _repo.Detalle_ListByRecepcionCompraAsync(idRecepcionCompra, ct);
        return new RecepcionCompraDto{
            IdRecepcionCompra=h.IdRecepcionCompra, Codigo=h.Codigo, IdCompra=h.IdCompra, IdAlmacenDestino=h.IdAlmacenDestino, FechaRecepcion=h.FechaRecepcion, Estado=h.Estado, Observacion=h.Observacion,
            Detalle=det.Select(d=>new RecepcionCompraDetalleDto{ IdRecepcionCompraDetalle=d.IdRecepcionCompraDetalle, IdCompraDetalle=d.IdCompraDetalle, IdItem=d.IdItem, CantidadRecibida=d.CantidadRecibida, Comentario=d.Comentario }).ToList()
        };
    }

    public async Task<RecepcionCompraRegistrarResponseDto> RegistrarAsync(RecepcionCompraRegistrarRequestDto req, CancellationToken ct)
    {
        if (req.IdCompra<=0) throw new ArgumentException("IdCompra inválido.");
        if (req.IdAlmacenDestino<=0) throw new ArgumentException("IdAlmacenDestino inválido.");
        if (req.Detalle==null || req.Detalle.Count==0) throw new ArgumentException("Detalle es obligatorio.");

        var det = req.Detalle.Select(d=> new RecepcionCompraDetalle{
            IdCompraDetalle=d.IdCompraDetalle, IdItem=d.IdItem, CantidadRecibida=d.CantidadRecibida, Comentario=string.IsNullOrWhiteSpace(d.Comentario)?null:d.Comentario.Trim()
        });

        var (id,cod)= await _repo.RegistrarAsync(req.IdCompra, req.IdAlmacenDestino, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), req.IdUsuario, det, ct);
        return new RecepcionCompraRegistrarResponseDto{ IdRecepcionCompra=id, Codigo=cod };
    }
}
