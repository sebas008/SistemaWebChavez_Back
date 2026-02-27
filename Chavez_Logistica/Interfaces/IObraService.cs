
using Chavez_Logistica.Dtos.Maestros.Obras;

namespace Chavez_Logistica.Interfaces;

public interface IObraService
{
    Task<List<ObraDto>> ListAsync(bool? soloActivos, CancellationToken ct);
    Task<ObraDto?> GetByIdAsync(int idObra, CancellationToken ct);
    Task<ObraCreateResponseDto> CrearAsync(ObraCreateRequestDto req, CancellationToken ct);
    Task ActualizarAsync(int idObra, ObraUpdateRequestDto req, CancellationToken ct);
}
