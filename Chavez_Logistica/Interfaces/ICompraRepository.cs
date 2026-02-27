using Chavez_Logistica.Entities.Logistica;

namespace Chavez_Logistica.Interfaces;

public interface ICompraRepository
{
    Task<IEnumerable<Compra>> ListAsync(int? idProveedor, int? idObra, string? estado, CancellationToken ct);
    Task<Compra?> GetByIdAsync(int idCompra, CancellationToken ct);
    Task<IEnumerable<CompraDetalle>> Detalle_ListByCompraAsync(int idCompra, CancellationToken ct);

    Task<(int IdCompra, string Codigo)> CrearAsync(int idProveedor, int idObra, string? observacion, int? idUsuario, IEnumerable<CompraDetalle> detalle, CancellationToken ct);
    Task CambiarEstadoAsync(int idCompra, string estado, int? idUsuario, string? observacion, CancellationToken ct);
}
