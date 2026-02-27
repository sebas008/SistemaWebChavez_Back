using Chavez_Logistica.Dtos.Logistica.RecepcionObra;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class RecepcionObraService : IRecepcionObraService
{
    private readonly IRecepcionObraRepository _repo;
    public RecepcionObraService(IRecepcionObraRepository repo) => _repo = repo;

    public async Task<List<RecepcionObraDto>> ListAsync(int? idOrdenFinal, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idOrdenFinal, estado, ct)).Select(r => new RecepcionObraDto{
            IdRecepcionObra=r.IdRecepcionObra, Codigo=r.Codigo, IdOrdenFinal=r.IdOrdenFinal, IdAlmacenOrigen=r.IdAlmacenOrigen, IdAlmacenDestino=r.IdAlmacenDestino, FechaRecepcion=r.FechaRecepcion, Estado=r.Estado, Observacion=r.Observacion
        }).ToList();

    public async Task<RecepcionObraDto?> GetByIdAsync(int idRecepcionObra, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idRecepcionObra, ct);
        if (h == null) return null;
        var det = await _repo.Detalle_ListByRecepcionObraAsync(idRecepcionObra, ct);
        return new RecepcionObraDto{
            IdRecepcionObra=h.IdRecepcionObra, Codigo=h.Codigo, IdOrdenFinal=h.IdOrdenFinal, IdAlmacenOrigen=h.IdAlmacenOrigen, IdAlmacenDestino=h.IdAlmacenDestino, FechaRecepcion=h.FechaRecepcion, Estado=h.Estado, Observacion=h.Observacion,
            Detalle=det.Select(d=>new RecepcionObraDetalleDto{ IdRecepcionObraDetalle=d.IdRecepcionObraDetalle, IdOrdenFinalDetalle=d.IdOrdenFinalDetalle, IdItem=d.IdItem, CantidadRecibida=d.CantidadRecibida, Comentario=d.Comentario }).ToList()
        };
    }

    public async Task<RecepcionObraRegistrarResponseDto> RegistrarAsync(RecepcionObraRegistrarRequestDto req, CancellationToken ct)
    {
        if (req.IdOrdenFinal<=0) throw new ArgumentException("IdOrdenFinal inválido.");
        if (req.IdAlmacenOrigen<=0) throw new ArgumentException("IdAlmacenOrigen inválido.");
        if (req.IdAlmacenDestino<=0) throw new ArgumentException("IdAlmacenDestino inválido.");
        if (req.Detalle==null || req.Detalle.Count==0) throw new ArgumentException("Detalle es obligatorio.");

        var det = req.Detalle.Select(d=> new RecepcionObraDetalle{
            IdOrdenFinalDetalle=d.IdOrdenFinalDetalle, IdItem=d.IdItem, CantidadRecibida=d.CantidadRecibida, Comentario=string.IsNullOrWhiteSpace(d.Comentario)?null:d.Comentario.Trim()
        });

        var (id,cod)= await _repo.RegistrarAsync(req.IdOrdenFinal, req.IdAlmacenOrigen, req.IdAlmacenDestino, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), req.IdUsuario, det, ct);
        return new RecepcionObraRegistrarResponseDto{ IdRecepcionObra=id, Codigo=cod };
    }
}
