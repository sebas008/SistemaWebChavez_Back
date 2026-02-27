using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> ListarAsync(CancellationToken ct);
        Task<Usuario?> ObtenerPorIdAsync(int idUsuario, CancellationToken ct);

        Task<int> CrearAsync(Usuario usuario, CancellationToken ct);
        Task ActualizarAsync(int idUsuario, Usuario usuario, CancellationToken ct);

        Task AsignarRolesAsync(int idUsuario, string rolesCsv, CancellationToken ct);
    }
}
