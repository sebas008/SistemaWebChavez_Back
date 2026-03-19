using Chavez_Logistica.Dtos.Maestros.Partida;

namespace Chavez_Logistica.Interfaces;

public interface IPartidaService
{
    Task<List<PartidaDto>> ListAsync(bool? soloActivas, CancellationToken ct);
    Task<PartidaDto?> GetByIdAsync(int idPartida, CancellationToken ct);
    Task<PartidaCreateResponseDto> CrearAsync(PartidaCreateRequestDto req, CancellationToken ct);
    Task ActualizarAsync(int idPartida, PartidaUpdateRequestDto req, CancellationToken ct);
}
