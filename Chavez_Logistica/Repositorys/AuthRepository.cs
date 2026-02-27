using System.Data;
using Dapper;
using Chavez_Logistica.Dtos.Auth;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnectionFactory _db;

    public AuthRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        string usuario,
        string password,
        CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        using var multi = await conn.QueryMultipleAsync(
            new CommandDefinition(
                "seguridad.usp_Auth_Login",
                new { Usuario = usuario, Password = password },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );

        var user = await multi.ReadFirstOrDefaultAsync<LoginUsuarioDto>();
        if (user == null)
            return null;

        var roles = (await multi.ReadAsync<LoginRolDto>()).ToList();
        var permisos = (await multi.ReadAsync<LoginPermisoDto>()).ToList();

        return new LoginResponseDto
        {
            Usuario = user,
            Roles = roles,
            Permisos = permisos
        };
    }
}