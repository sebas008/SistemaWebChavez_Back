using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces
{
    public interface IUnidadMedidaRepository
    {
        Task<IEnumerable<UnidadMedida>> ListAsync(CancellationToken ct);
        Task<UnidadMedida?> GetByIdAsync(int idUnidadMedida, CancellationToken ct);
        Task<int> CrearAsync(UnidadMedida entity, CancellationToken ct);
        Task ActualizarAsync(int idUnidadMedida, UnidadMedida entity, CancellationToken ct);
    }
}
