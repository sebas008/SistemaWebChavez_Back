using System.Data;
using Dapper;
using Chavez_Logistica.Dtos.Logistica.Atencion;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;
public class AtencionRepository : IAtencionRepository
{
    private readonly IDbConnectionFactory _db;
    public AtencionRepository(IDbConnectionFactory db) => _db = db;

    public async Task<List<AtencionBandejaDto>> ListarBandejaAsync(CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        var rows = await conn.QueryAsync<AtencionBandejaDto>(
            new CommandDefinition("logistica.usp_Bandeja_AlmacenInternoDesdeRequerimiento_List",
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
        return rows.ToList();
    }
}
