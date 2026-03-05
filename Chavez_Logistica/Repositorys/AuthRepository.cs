using System.Data;
using Dapper;
using Chavez_Logistica.Dtos.Auth;
using Chavez_Logistica.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chavez_Logistica.Repositorys;

public class AuthRepository : IAuthRepository
{
    private readonly IDbConnectionFactory _db;

    public AuthRepository(IDbConnectionFactory db)
    {
        _db = db;
    }

    public async Task<LoginResponseDto?> LoginAsync(string usuario, string password, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        // Probamos firmas conocidas SIN mandar parámetros extra.
        // Esto evita:
        // - '@Password is not a parameter...'
        // - 'expects parameter @UsuarioLogin which was not supplied'
        // Y NO hardcodea usuarios.

        // 1) Firma más común: (@UsuarioLogin, @Password)
        var res = await TryLogin(conn, "seguridad.usp_Auth_Login",
            new (string, object?)[] { ("@UsuarioLogin", usuario), ("@Password", password) }, ct);

        if (res != null) return res;

        // 2) Variación: (@Usuario, @Password)
        res = await TryLogin(conn, "seguridad.usp_Auth_Login",
            new (string, object?)[] { ("@Usuario", usuario), ("@Password", password) }, ct);

        if (res != null) return res;

        // 3) Variación: (@UsuarioLogin, @PasswordInput)
        res = await TryLogin(conn, "seguridad.usp_Auth_Login",
            new (string, object?)[] { ("@UsuarioLogin", usuario), ("@PasswordInput", password) }, ct);

        if (res != null) return res;

        // 4) Variación: (@Usuario, @PasswordInput)
        res = await TryLogin(conn, "seguridad.usp_Auth_Login",
            new (string, object?)[] { ("@Usuario", usuario), ("@PasswordInput", password) }, ct);

        return res;
    }

    private static async Task<LoginResponseDto?> TryLogin(
        IDbConnection conn,
        string spName,
        (string name, object? value)[] parameters,
        CancellationToken ct)
    {
        try
        {
            var p = new DynamicParameters();
            foreach (var (name, value) in parameters)
                p.Add(name, value);

            using var multi = await conn.QueryMultipleAsync(
                new CommandDefinition(
                    spName,
                    p,
                    commandType: CommandType.StoredProcedure,
                    cancellationToken: ct
                )
            );

            var user = await multi.ReadFirstOrDefaultAsync<LoginUsuarioDto>();
            if (user == null) return null;

            // Roles/Permisos: solo si el SP realmente devuelve más resultsets
            var roles = multi.IsConsumed ? new List<LoginRolDto>() : (await multi.ReadAsync<LoginRolDto>()).AsList();
            var permisos = multi.IsConsumed ? new List<LoginPermisoDto>() : (await multi.ReadAsync<LoginPermisoDto>()).AsList();

            return new LoginResponseDto
            {
                Usuario = user,
                Roles = roles,
                Permisos = permisos
            };
        }
        catch (SqlException ex)
        {
            // Firma incorrecta / parámetro no existe / falta parámetro:
            // - 8144 / 8145: too many args / wrong args
            // - 201: expects parameter not supplied
            // - 8146: has no parameters and arguments were supplied (caso parecido en otros SPs)
            if (ex.Number is 201 or 8144 or 8145 or 8146)
                return null;

            throw; // otros errores sí deben escalar
        }
    }
}