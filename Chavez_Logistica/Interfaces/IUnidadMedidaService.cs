using Chavez_Logistica.Dtos.Maestros.UnidadMedida;

namespace Chavez_Logistica.Interfaces
{
    public interface IUnidadMedidaService
    {
        Task<List<UnidadMedidaDto>> ListAsync(CancellationToken ct);
        Task<UnidadMedidaDto?> GetByIdAsync(int idUnidadMedida, CancellationToken ct);
        Task<UnidadMedidaCreateResponseDto> CrearAsync(UnidadMedidaCreateRequestDto req, CancellationToken ct);
        Task ActualizarAsync(int idUnidadMedida, UnidadMedidaUpdateRequestDto req, CancellationToken ct);
    }
}
