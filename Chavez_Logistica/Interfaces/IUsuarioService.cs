using Chavez_Logistica.Dtos.Usuarios;

namespace Chavez_Logistica.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ListAsync(CancellationToken ct);
        Task<UsuarioDto?> GetByIdAsync(int idUsuario, CancellationToken ct);

        Task<UsuarioCreateResponseDto> CreateAsync(UsuarioCreateRequestDto req, CancellationToken ct);
        Task UpdateAsync(int idUsuario, UsuarioUpdateRequestDto req, CancellationToken ct);

        Task AssignRolesAsync(int idUsuario, UsuarioAsignarRolesRequestDto req, CancellationToken ct);
    }
}
