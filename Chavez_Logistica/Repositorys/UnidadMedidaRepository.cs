using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;
using Dapper;
using System.Data;

namespace Chavez_Logistica.Repositorys;

public class UnidadMedidaRepository : IUnidadMedidaRepository
{
    private readonly IDbConnectionFactory _db;
    public UnidadMedidaRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<UnidadMedida>> ListAsync(CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<UnidadMedida>(
            new CommandDefinition(
                "maestros.usp_UnidadMedida_List",
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<UnidadMedida?> GetByIdAsync(int idUnidadMedida, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<UnidadMedida>(
            new CommandDefinition(
                "maestros.usp_UnidadMedida_GetById",
                new { IdUnidadMedida = idUnidadMedida },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<int> CrearAsync(UnidadMedida entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                "maestros.usp_UnidadMedida_Crear",
                new { entity.Codigo, entity.Nombre },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task ActualizarAsync(int idUnidadMedida, string nombre, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "maestros.usp_UnidadMedida_Actualizar",
                new { IdUnidadMedida = idUnidadMedida, Nombre = nombre },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }
}