using Chavez_Logistica.Entities.Logistica;

namespace Chavez_Logistica.Interfaces;

public interface IRecepcionObraRepository
{
    Task<IEnumerable<RecepcionObra>> ListAsync(int? idOrdenFinal, string? estado, CancellationToken ct);
    Task<RecepcionObra?> GetByIdAsync(int idRecepcionObra, CancellationToken ct);
    Task<IEnumerable<RecepcionObraDetalle>> Detalle_ListByRecepcionObraAsync(int idRecepcionObra, CancellationToken ct);

    Task<(int IdRecepcionObra, string Codigo)> RegistrarAsync(int idOrdenFinal, int idAlmacenOrigen, int idAlmacenDestino, string? observacion, int? idUsuario, IEnumerable<RecepcionObraDetalle> detalle, CancellationToken ct);
}
