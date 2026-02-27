using Chavez_Logistica.Dtos.Logistica.Atencion;

namespace Chavez_Logistica.Interfaces;

public interface IAtencionService
{
    Task<List<AtencionDto>> ListAsync(int? idObra, int? idAlmacenOrigen, int? idAlmacenDestino, string? estado, CancellationToken ct);
    Task<AtencionDto?> GetByIdAsync(int idAtencion, CancellationToken ct);
    Task<AtencionRegistrarResponseDto> RegistrarDesdeAlmacenInternoAsync(AtencionRegistrarRequestDto req, CancellationToken ct);
}
