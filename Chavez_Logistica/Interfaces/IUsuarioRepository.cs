using Chavez_Logistica.Entities;

namespace Chavez_Logistica.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<List<Usuario>> ListAsync(CancellationToken ct);
        Task<Usuario?> GetByIdAsync(int idUsuario, CancellationToken ct);

        Task<int> CreateAsync(Usuario usuario, CancellationToken ct);
        Task UpdateAsync(int idUsuario, Usuario usuario, CancellationToken ct);

        Task AssignRolesAsync(int idUsuario, List<string> roles, CancellationToken ct);
    }
}
