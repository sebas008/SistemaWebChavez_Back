using Chavez_Logistica.Entities.Logistica;

namespace Chavez_Logistica.Interfaces;

public interface IAtencionRepository
{
    Task<IEnumerable<Atencion>> ListAsync(int? idObra, int? idAlmacenOrigen, int? idAlmacenDestino, string? estado, CancellationToken ct);
    Task<Atencion?> GetByIdAsync(int idAtencion, CancellationToken ct);
    Task<IEnumerable<AtencionDetalle>> Detalle_ListByAtencionAsync(int idAtencion, CancellationToken ct);

    Task<(int IdAtencion, string Codigo)> RegistrarDesdeAlmacenInternoAsync(
        int idObra, int idAlmacenOrigen, int idAlmacenDestino, string metodoAtencion, string? observacion, int? idUsuario,
        IEnumerable<AtencionDetalle> detalle, CancellationToken ct);
}
