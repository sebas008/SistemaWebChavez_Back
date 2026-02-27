using Chavez_Logistica.Dtos.Logistica.Atencion;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class AtencionService : IAtencionService
{
    private readonly IAtencionRepository _repo;
    public AtencionService(IAtencionRepository repo) => _repo = repo;

    public async Task<List<AtencionDto>> ListAsync(int? idObra, int? idAlmacenOrigen, int? idAlmacenDestino, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idObra, idAlmacenOrigen, idAlmacenDestino, estado, ct)).Select(a => new AtencionDto{
            IdAtencion=a.IdAtencion, Codigo=a.Codigo, Fecha=a.Fecha, IdObra=a.IdObra, IdAlmacenOrigen=a.IdAlmacenOrigen, IdAlmacenDestino=a.IdAlmacenDestino,
            MetodoAtencion=a.MetodoAtencion, Estado=a.Estado, Observacion=a.Observacion
        }).ToList();

    public async Task<AtencionDto?> GetByIdAsync(int idAtencion, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idAtencion, ct);
        if (h == null) return null;
        var det = await _repo.Detalle_ListByAtencionAsync(idAtencion, ct);
        return new AtencionDto{
            IdAtencion=h.IdAtencion, Codigo=h.Codigo, Fecha=h.Fecha, IdObra=h.IdObra, IdAlmacenOrigen=h.IdAlmacenOrigen, IdAlmacenDestino=h.IdAlmacenDestino,
            MetodoAtencion=h.MetodoAtencion, Estado=h.Estado, Observacion=h.Observacion,
            Detalle=det.Select(d=>new AtencionDetalleDto{ IdAtencionDetalle=d.IdAtencionDetalle, IdItem=d.IdItem, CantidadPlanificada=d.CantidadPlanificada, CantidadAtendida=d.CantidadAtendida, Comentario=d.Comentario }).ToList()
        };
    }

    public async Task<AtencionRegistrarResponseDto> RegistrarDesdeAlmacenInternoAsync(AtencionRegistrarRequestDto req, CancellationToken ct)
    {
        if (req.IdObra<=0) throw new ArgumentException("IdObra inválido.");
        if (req.IdAlmacenOrigen<=0) throw new ArgumentException("IdAlmacenOrigen inválido.");
        if (req.IdAlmacenDestino<=0) throw new ArgumentException("IdAlmacenDestino inválido.");
        if (string.IsNullOrWhiteSpace(req.MetodoAtencion)) throw new ArgumentException("MetodoAtencion es obligatorio.");
        if (req.Detalle==null || req.Detalle.Count==0) throw new ArgumentException("Detalle es obligatorio.");

        var det = req.Detalle.Select(d=> new AtencionDetalle{
            IdItem=d.IdItem, CantidadPlanificada=d.CantidadPlanificada, CantidadAtendida=d.CantidadAtendida,
            Comentario=string.IsNullOrWhiteSpace(d.Comentario)?null:d.Comentario.Trim()
        });

        var (id,cod)= await _repo.RegistrarDesdeAlmacenInternoAsync(req.IdObra, req.IdAlmacenOrigen, req.IdAlmacenDestino,
            req.MetodoAtencion.Trim().ToUpperInvariant(),
            string.IsNullOrWhiteSpace(req.Observacion)?null:req.Observacion.Trim(),
            req.IdUsuario, det, ct);

        return new AtencionRegistrarResponseDto{ IdAtencion=id, Codigo=cod };
    }
}
