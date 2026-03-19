using System.Data;
using Dapper;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class PartidaRepository : IPartidaRepository
{
    private readonly IDbConnectionFactory _db;
    public PartidaRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Partida>> ListAsync(bool? soloActivas, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Partida>(new CommandDefinition(
            "maestros.usp_Partida_List",
            new { SoloActivas = soloActivas },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct
        ));
    }

    public async Task<Partida?> GetByIdAsync(int idPartida, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Partida>(new CommandDefinition(
            "maestros.usp_Partida_GetById",
            new { IdPartida = idPartida },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct
        ));
    }

    public async Task<int> CrearAsync(string nombre, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(new CommandDefinition(
            "maestros.usp_Partida_Crear",
            new { Nombre = nombre },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct
        ));
    }

    public async Task ActualizarAsync(int idPartida, string nombre, bool activo, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(
            "maestros.usp_Partida_Actualizar",
            new { IdPartida = idPartida, Nombre = nombre, Activo = activo },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct
        ));
    }
}
