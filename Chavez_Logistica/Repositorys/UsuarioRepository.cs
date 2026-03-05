using System.Data;
using Dapper;
using Chavez_Logistica.Interfaces;
using Chavez_Logistica.Entities; 

namespace Chavez_Logistica.Repositorys;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly IDbConnectionFactory _db;

    public UsuarioRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Usuario>> ListarAsync(CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        return await conn.QueryAsync<Usuario>(
            new CommandDefinition(
                "seguridad.usp_Usuario_Listar",
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int idUsuario, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<Usuario>(
            new CommandDefinition(
                "seguridad.usp_Usuario_ObtenerPorId",
                new { IdUsuario = idUsuario },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<int> CrearAsync(Usuario usuario, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        // SP tolerante: acepta @UsuarioLogin o @Usuario, y PasswordHash si existe
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                "seguridad.usp_Usuario_Crear",
                new
                {
                    UsuarioLogin = usuario.UsuarioLogin,
                    Usuario = usuario.UsuarioLogin,
                    usuario.Nombres,
                    usuario.Email,
                    usuario.PasswordHash,
                    usuario.PasswordSalt
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task ActualizarAsync(int idUsuario, Usuario usuario, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        // SP espera: @IdUsuario, @Nombres, @Email, @Activo
        await conn.ExecuteAsync(
            new CommandDefinition(
                "seguridad.usp_Usuario_Actualizar",
                new
                {
                    IdUsuario = idUsuario,
                    usuario.Nombres,
                    usuario.Email,
                    usuario.Activo
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task AsignarRolesAsync(int idUsuario, string rolesCsv, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        // SP espera: @IdUsuario, @RolesCsv
        await conn.ExecuteAsync(
            new CommandDefinition(
                "seguridad.usp_Usuario_AsignarRoles",
                new
                {
                    IdUsuario = idUsuario,
                    RolesCsv = rolesCsv
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<IEnumerable<string>> GetRolesAsync(int idUsuario, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        // Lee roles directamente (no depende de SPs)
        const string sql = @"
SELECT r.Codigo
FROM seguridad.UsuarioRol ur
INNER JOIN seguridad.Rol r ON r.IdRol = ur.IdRol
WHERE ur.IdUsuario = @IdUsuario
  AND ISNULL(ur.Activo,1) = 1
  AND ISNULL(r.Activo,1) = 1
ORDER BY r.Codigo;";

        return await conn.QueryAsync<string>(
            new CommandDefinition(
                sql,
                new { IdUsuario = idUsuario },
                commandType: CommandType.Text,
                cancellationToken: ct
            )
        );
    }

}
