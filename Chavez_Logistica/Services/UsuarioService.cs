using Chavez_Logistica.Dtos.Usuarios;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;
using System.Security.Cryptography;
using System.Text;

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

            // Password obligatorio para creación. Si viene vacío, usa 123456.
            var pw = (req.Password ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(pw))
                pw = "123456";

            // SHA2_256 compatible con SQL: HASHBYTES('SHA2_256', ...)
            entity.PasswordHash = SHA256.HashData(Encoding.UTF8.GetBytes(pw));
            entity.PasswordSalt = null;

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

        public async Task<List<string>> GetRolesAsync(int idUsuario, CancellationToken ct)
        {
            var roles = await _repo.GetRolesAsync(idUsuario, ct);
            return roles
                .Select(r => (r ?? string.Empty).Trim().ToUpperInvariant())
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .ToList();
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