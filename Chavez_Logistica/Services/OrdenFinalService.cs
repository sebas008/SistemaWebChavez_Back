using Chavez_Logistica.Dtos.Logistica.OrdenFinal;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class OrdenFinalService : IOrdenFinalService
{
    private readonly IOrdenFinalRepository _repo;
    public OrdenFinalService(IOrdenFinalRepository repo) => _repo = repo;

    public async Task<List<OrdenFinalDto>> ListAsync(int? idObra, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idObra, estado, ct)).Select(o => new OrdenFinalDto{
            IdOrdenFinal=o.IdOrdenFinal, Codigo=o.Codigo, IdRequerimiento=o.IdRequerimiento, IdObra=o.IdObra, Fecha=o.Fecha, Estado=o.Estado, Observacion=o.Observacion
        }).ToList();

    public async Task<OrdenFinalDto?> GetByIdAsync(int idOrdenFinal, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idOrdenFinal, ct);
        if (h == null) return null;
        var det = await _repo.Detalle_ListByOrdenFinalAsync(idOrdenFinal, ct);
        return new OrdenFinalDto{
            IdOrdenFinal=h.IdOrdenFinal, Codigo=h.Codigo, IdRequerimiento=h.IdRequerimiento, IdObra=h.IdObra, Fecha=h.Fecha, Estado=h.Estado, Observacion=h.Observacion,
            Detalle=det.Select(d=>new OrdenFinalDetalleDto{ IdOrdenFinalDetalle=d.IdOrdenFinalDetalle, IdItem=d.IdItem, Cantidad=d.Cantidad, Observacion=d.Observacion }).ToList()
        };
    }

    public async Task<OrdenFinalCreateResponseDto> CrearAsync(OrdenFinalCreateRequestDto req, CancellationToken ct)
    {
        if (req.IdRequerimiento<=0) throw new ArgumentException("IdRequerimiento inválido.");
        if (req.Detalle==null || req.Detalle.Count==0) throw new ArgumentException("Detalle es obligatorio.");
        var det = req.Detalle.Select(d=> new OrdenFinalDetalle{ IdItem=d.IdItem, Cantidad=d.Cantidad, Observacion=string.IsNullOrWhiteSpace(d.Observacion)?null:d.Observacion.Trim() });
        var (id,cod)= await _repo.CrearAsync(req.IdRequerimiento, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), req.IdUsuario, det, ct);
        return new OrdenFinalCreateResponseDto{ IdOrdenFinal=id, Codigo=cod };
    }

    public async Task CambiarEstadoAsync(int idOrdenFinal, OrdenFinalCambiarEstadoRequestDto req, CancellationToken ct)
        => await _repo.CambiarEstadoAsync(idOrdenFinal, req.Estado.Trim().ToUpperInvariant(), req.IdUsuario, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), ct);
}
