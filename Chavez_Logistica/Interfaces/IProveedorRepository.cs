using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces;

public interface IProveedorRepository
{
    Task<IEnumerable<Proveedor>> ListAsync(bool? soloActivos, CancellationToken ct);
    Task<Proveedor?> GetByIdAsync(int idProveedor, CancellationToken ct);
    Task<int> CrearAsync(Proveedor entity, CancellationToken ct);
    Task ActualizarAsync(int idProveedor, Proveedor entity, CancellationToken ct);
}
