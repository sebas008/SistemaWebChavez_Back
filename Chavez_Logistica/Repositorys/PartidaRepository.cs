using System.Data;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

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
            cancellationToken: ct));
    }

    public async Task<Partida?> GetByIdAsync(int idPartida, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Partida>(new CommandDefinition(
            "maestros.usp_Partida_GetById",
            new { IdPartida = idPartida },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task<int> CrearAsync(Partida entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(new CommandDefinition(
            "maestros.usp_Partida_Crear",
            new { entity.Nombre },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task ActualizarAsync(int idPartida, Partida entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(
            "maestros.usp_Partida_Actualizar",
            new { IdPartida = idPartida, entity.Nombre, Activo = entity.Activo },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }
}
