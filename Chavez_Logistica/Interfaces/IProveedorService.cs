using Chavez_Logistica.Dtos.Maestros.Proveedor;

namespace Chavez_Logistica.Interfaces;

public interface IProveedorService
{
    Task<List<ProveedorDto>> ListAsync(bool? soloActivos, CancellationToken ct);
    Task<ProveedorDto?> GetByIdAsync(int idProveedor, CancellationToken ct);
    Task<ProveedorCreateResponseDto> CrearAsync(ProveedorCreateRequestDto req, CancellationToken ct);
    Task ActualizarAsync(int idProveedor, ProveedorUpdateRequestDto req, CancellationToken ct);
}
