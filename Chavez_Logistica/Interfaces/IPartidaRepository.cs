using Chavez_Logistica.Entities;
namespace Chavez_Logistica.Interfaces;
public interface IPartidaRepository
{
    Task<IEnumerable<Partida>> ListAsync(bool? soloActivas, CancellationToken ct);
    Task<Partida?> GetByIdAsync(int idPartida, CancellationToken ct);
    Task<int> CrearAsync(Partida entity, CancellationToken ct);
    Task ActualizarAsync(int idPartida, Partida entity, CancellationToken ct);
}
