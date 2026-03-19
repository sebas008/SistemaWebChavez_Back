using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces;

public interface IPartidaRepository
{
    Task<IEnumerable<Partida>> ListAsync(bool? soloActivas, CancellationToken ct);
    Task<Partida?> GetByIdAsync(int idPartida, CancellationToken ct);
    Task<int> CrearAsync(string nombre, CancellationToken ct);
    Task ActualizarAsync(int idPartida, string nombre, bool activo, CancellationToken ct);
}
