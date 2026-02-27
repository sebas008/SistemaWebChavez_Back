using Chavez_Logistica.Entities.Logistica;

namespace Chavez_Logistica.Interfaces;

public interface IRecepcionCompraRepository
{
    Task<IEnumerable<RecepcionCompra>> ListAsync(int? idCompra, string? estado, CancellationToken ct);
    Task<RecepcionCompra?> GetByIdAsync(int idRecepcionCompra, CancellationToken ct);
    Task<IEnumerable<RecepcionCompraDetalle>> Detalle_ListByRecepcionCompraAsync(int idRecepcionCompra, CancellationToken ct);

    Task<(int IdRecepcionCompra, string Codigo)> RegistrarAsync(int idCompra, int idAlmacenDestino, string? observacion, int? idUsuario, IEnumerable<RecepcionCompraDetalle> detalle, CancellationToken ct);
}
