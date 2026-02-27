using System.Data;
using Dapper;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class ObraRepository : IObraRepository
{
    private readonly IDbConnectionFactory _db;
    public ObraRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Obra>> ListAsync(bool? soloActivos, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Obra>(new CommandDefinition(
            "maestros.usp_Obra_List",
            new { SoloActivos = soloActivos },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task<Obra?> GetByIdAsync(int idObra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Obra>(new CommandDefinition(
            "maestros.usp_Obra_GetById",
            new { IdObra = idObra },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task<int> CrearAsync(Obra entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(new CommandDefinition(
            "maestros.usp_Obra_Crear",
            new { entity.Nombre, entity.Ubicacion },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task ActualizarAsync(int idObra, Obra entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(
            "maestros.usp_Obra_Actualizar",
            new { IdObra = idObra, entity.Nombre, entity.Ubicacion, entity.Activa },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }
}
