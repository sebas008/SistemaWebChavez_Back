using Chavez_Logistica.Dtos.Logistica.RecepcionCompra;

namespace Chavez_Logistica.Interfaces;

public interface IRecepcionCompraService
{
    Task<List<RecepcionCompraDto>> ListAsync(int? idCompra, string? estado, CancellationToken ct);
    Task<RecepcionCompraDto?> GetByIdAsync(int idRecepcionCompra, CancellationToken ct);
    Task<RecepcionCompraRegistrarResponseDto> RegistrarAsync(RecepcionCompraRegistrarRequestDto req, CancellationToken ct);
}
