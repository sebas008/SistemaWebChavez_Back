using Chavez_Logistica.Dtos.Logistica.RecepcionObra;

namespace Chavez_Logistica.Interfaces;

public interface IRecepcionObraService
{
    Task<List<RecepcionObraDto>> ListAsync(int? idOrdenFinal, string? estado, CancellationToken ct);
    Task<RecepcionObraDto?> GetByIdAsync(int idRecepcionObra, CancellationToken ct);
    Task<RecepcionObraRegistrarResponseDto> RegistrarAsync(RecepcionObraRegistrarRequestDto req, CancellationToken ct);
}
