using Chavez_Logistica.Dtos.Logistica.Requerimiento;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class RequerimientoService : IRequerimientoService
{
    private readonly IRequerimientoRepository _repo;
    private readonly IUsuarioRepository _usuarios;

    public RequerimientoService(IRequerimientoRepository repo, IUsuarioRepository usuarios)
    {
        _repo = repo;
        _usuarios = usuarios;
    }

    public async Task<List<RequerimientoDto>> ListAsync(int? idObra, string? estado, CancellationToken ct)
        => (await _repo.ListAsync(idObra, estado, ct)).Select(r => new RequerimientoDto
        {
            IdRequerimiento = r.IdRequerimiento,
            Codigo = r.Codigo,
            IdObra = r.IdObra,
            FechaSolicitud = r.FechaSolicitud,
            Estado = r.Estado,
            Observacion = r.Observacion
        }).ToList();

    public async Task<RequerimientoDto?> GetByIdAsync(int idRequerimiento, CancellationToken ct)
    {
        var h = await _repo.GetByIdAsync(idRequerimiento, ct);
        if (h == null) return null;

        var det = await _repo.Detalle_ListByRequerimientoAsync(idRequerimiento, ct);

        return new RequerimientoDto
        {
            IdRequerimiento = h.IdRequerimiento,
            Codigo = h.Codigo,
            IdObra = h.IdObra,
            FechaSolicitud = h.FechaSolicitud,
            Estado = h.Estado,
            Observacion = h.Observacion,
            Detalle = det.Select(d => new RequerimientoDetalleDto
            {
                IdRequerimientoDetalle = d.IdRequerimientoDetalle,
                IdItem = d.IdItem,
                IdPartida = d.IdPartida,
                Cantidad = d.Cantidad,
                IdUnidadMedida = d.IdUnidadMedida,
                Comentario = d.Comentario,
                Observacion = d.Observacion,
                Destino = d.Destino
            }).ToList()
        };
    }

    public async Task<RequerimientoCreateResponseDto> CrearAsync(RequerimientoCreateRequestDto req, CancellationToken ct)
    {
        if (req.IdObra <= 0) throw new ArgumentException("IdObra inválido.");
        if (req.Detalle == null || req.Detalle.Count == 0) throw new ArgumentException("Detalle es obligatorio.");

        if (req.IdUsuario.HasValue)
        {
            var roles = (await _usuarios.GetRolesAsync(req.IdUsuario.Value, ct))
                .Select(r => (r ?? string.Empty).Trim().ToUpperInvariant())
                .ToHashSet();

            if (roles.Contains("LOGISTICA") && !roles.Contains("MASTER") && !roles.Contains("OBRAS") && !roles.Contains("OFICINA_TECNICA"))
                throw new InvalidOperationException("Logística no puede crear requerimientos; solo puede ver la bandeja y definir destino por ítem.");
        }

        var det = req.Detalle.Select(d => new RequerimientoDetalle
        {
            IdItem = d.IdItem,
            IdPartida = d.IdPartida,
            Cantidad = d.Cantidad,
            IdUnidadMedida = d.IdUnidadMedida,
            Comentario = string.IsNullOrWhiteSpace(d.Comentario) ? null : d.Comentario.Trim(),
            Observacion = string.IsNullOrWhiteSpace(d.Observacion) ? null : d.Observacion.Trim()
        });

        var result = await _repo.CrearAsync(
            req.IdObra,
            string.IsNullOrWhiteSpace(req.Observacion) ? null : req.Observacion.Trim(),
            req.IdUsuario,
            det,
            ct
        );

        return new RequerimientoCreateResponseDto
        {
            IdRequerimiento = result.IdRequerimiento,
            Codigo = result.Codigo
        };
    }

    public async Task CambiarEstadoAsync(int idRequerimiento, RequerimientoCambiarEstadoRequestDto req, CancellationToken ct)
        => await _repo.CambiarEstadoAsync(
            idRequerimiento,
            req.Estado.Trim().ToUpperInvariant(),
            req.IdUsuario,
            string.IsNullOrWhiteSpace(req.Observacion) ? null : req.Observacion.Trim(),
            ct);

    public async Task AsignarDestinoDetalleAsync(int idRequerimientoDetalle, RequerimientoDetalleDestinoRequestDto req, CancellationToken ct)
    {
        var destino = string.IsNullOrWhiteSpace(req.Destino) ? null : req.Destino.Trim().ToUpperInvariant();

        if (destino is not null && destino is not ("COMPRA" or "ALMACEN_INTERNO"))
            throw new ArgumentException("Destino inválido.");

        await _repo.AsignarDestinoDetalleAsync(idRequerimientoDetalle, destino, req.IdUsuario, ct);
    }

    public async Task CambiarEstadoDetalleAsync(int idRequerimientoDetalle, RequerimientoDetalleCambiarEstadoRequestDto req, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(req.Estado)) throw new ArgumentException("Estado es obligatorio.");

        await _repo.CambiarEstadoDetalleAsync(
            idRequerimientoDetalle,
            req.Estado.Trim().ToUpperInvariant(),
            req.EntregaATiempo,
            ct);
    }
}
