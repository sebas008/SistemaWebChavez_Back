using Chavez_Logistica.Dtos.Usuarios;

namespace Chavez_Logistica.Interfaces
{
    public interface IUsuarioService
    {
        Task<List<UsuarioDto>> ListarAsync(CancellationToken ct);
        Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario, CancellationToken ct);

        Task<UsuarioCreateResponseDto> CrearAsync(UsuarioCreateRequestDto req, CancellationToken ct);
        Task ActualizarAsync(int idUsuario, UsuarioUpdateRequestDto req, CancellationToken ct);

        Task AsignarRolesAsync(int idUsuario, UsuarioAsignarRolesRequestDto req, CancellationToken ct);
    }
}
