using Chavez_Logistica.Dtos.Logistica.Compra;

namespace Chavez_Logistica.Interfaces;

public interface ICompraService
{
    Task<List<CompraDto>> ListAsync(int? idProveedor, int? idObra, string? estado, CancellationToken ct);
    Task<CompraDto?> GetByIdAsync(int idCompra, CancellationToken ct);

    Task<CompraCreateResponseDto> CrearAsync(CompraCreateRequestDto req, CancellationToken ct);
    Task CambiarEstadoAsync(int idCompra, CompraCambiarEstadoRequestDto req, CancellationToken ct);
}
