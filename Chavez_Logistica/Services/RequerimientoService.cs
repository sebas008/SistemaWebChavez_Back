using Chavez_Logistica.Dtos.Logistica.Requerimiento;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class RequerimientoService : IRequerimientoService
{
    private readonly IRequerimientoRepository _repo;
    public RequerimientoService(IRequerimientoRepository repo) => _repo = repo;

    public async Task<List<RequerimientoDto>> ListAsync(int? idObra, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idObra, estado, ct)).Select(r => new RequerimientoDto{
            IdRequerimiento=r.IdRequerimiento, Codigo=r.Codigo, IdObra=r.IdObra, FechaSolicitud=r.FechaSolicitud, Estado=r.Estado, Observacion=r.Observacion
        }).ToList();

    public async Task<RequerimientoDto?> GetByIdAsync(int idRequerimiento, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idRequerimiento, ct);
        if (h == null) return null;
        var det = await _repo.Detalle_ListByRequerimientoAsync(idRequerimiento, ct);
        return new RequerimientoDto{
            IdRequerimiento=h.IdRequerimiento, Codigo=h.Codigo, IdObra=h.IdObra, FechaSolicitud=h.FechaSolicitud, Estado=h.Estado, Observacion=h.Observacion,
            Detalle=det.Select(d=>new RequerimientoDetalleDto{ IdRequerimientoDetalle=d.IdRequerimientoDetalle, IdItem=d.IdItem, Cantidad=d.Cantidad, Observacion=d.Observacion }).ToList()
        };
    }

    public async Task<RequerimientoCreateResponseDto> CrearAsync(RequerimientoCreateRequestDto req, CancellationToken ct)
    {
        if (req.IdObra<=0) throw new ArgumentException("IdObra inválido.");
        if (req.Detalle==null || req.Detalle.Count==0) throw new ArgumentException("Detalle es obligatorio.");
        var det = req.Detalle.Select(d=> new RequerimientoDetalle{ IdItem=d.IdItem, Cantidad=d.Cantidad, Observacion=string.IsNullOrWhiteSpace(d.Observacion)?null:d.Observacion.Trim() });
        var (id,cod)= await _repo.CrearAsync(req.IdObra, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), req.IdUsuario, det, ct);
        return new RequerimientoCreateResponseDto{ IdRequerimiento=id, Codigo=cod };
    }

    public async Task CambiarEstadoAsync(int idRequerimiento, RequerimientoCambiarEstadoRequestDto req, CancellationToken ct)
        => await _repo.CambiarEstadoAsync(idRequerimiento, req.Estado.Trim().ToUpperInvariant(), req.IdUsuario, string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(), ct);
}
