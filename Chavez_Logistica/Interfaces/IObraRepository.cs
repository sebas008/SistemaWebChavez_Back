using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces;

public interface IObraRepository
{
    Task<IEnumerable<Obra>> ListAsync(bool? soloActivos, CancellationToken ct);
    Task<Obra?> GetByIdAsync(int idObra, CancellationToken ct);
    Task<int> CrearAsync(Obra entity, CancellationToken ct);
    Task ActualizarAsync(int idObra, Obra entity, CancellationToken ct);
}
