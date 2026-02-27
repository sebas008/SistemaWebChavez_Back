using Chavez_Logistica.Dtos.Usuarios;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repo;

        public UsuarioService(IUsuarioRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UsuarioDto>> ListarAsync(CancellationToken ct)
        {
            var usuarios = await _repo.ListarAsync(ct);
            return usuarios.Select(MapToDto).ToList();
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(int idUsuario, CancellationToken ct)
        {
            var usuario = await _repo.ObtenerPorIdAsync(idUsuario, ct);
            return usuario == null ? null : MapToDto(usuario);
        }

        public async Task<UsuarioCreateResponseDto> CrearAsync(UsuarioCreateRequestDto req, CancellationToken ct)
        {
            var entity = new Usuario
            {
                UsuarioLogin = req.UsuarioLogin.Trim(),
                Nombres = req.Nombres.Trim(),
                Email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim(),
                Activo = true
            };

            var id = await _repo.CrearAsync(entity, ct);

            return new UsuarioCreateResponseDto
            {
                IdUsuario = id
            };
        }

        public async Task ActualizarAsync(int idUsuario, UsuarioUpdateRequestDto req, CancellationToken ct)
        {
            var entity = new Usuario
            {
                Nombres = req.Nombres.Trim(),
                Email = string.IsNullOrWhiteSpace(req.Email) ? null : req.Email.Trim(),
                Activo = req.Activo
            };

            await _repo.ActualizarAsync(idUsuario, entity, ct);
        }

        public async Task AsignarRolesAsync(int idUsuario, UsuarioAsignarRolesRequestDto req, CancellationToken ct)
        {
            // arma CSV para el SP (ADMIN,OPERADOR,...)
            var rolesCsv = string.Join(",",
                (req.Roles ?? new List<string>())
                    .Where(r => !string.IsNullOrWhiteSpace(r))
                    .Select(r => r.Trim().ToUpperInvariant())
            );

            await _repo.AsignarRolesAsync(idUsuario, rolesCsv, ct);
        }

        private static UsuarioDto MapToDto(Usuario u)
        {
            return new UsuarioDto
            {
                IdUsuario = u.IdUsuario,
                UsuarioLogin = u.UsuarioLogin,
                Nombres = u.Nombres,
                Email = u.Email,
                Activo = u.Activo,
                FechaCreacion = u.FechaCreacion
            };
        }
    }
}