using System.Data;
using Dapper;
using Chavez_Logistica.Dtos.Logistica.Compra;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;
public class CompraRepository : ICompraRepository
{
    private readonly IDbConnectionFactory _db;
    public CompraRepository(IDbConnectionFactory db) => _db = db;

    public async Task<List<CompraBandejaDto>> ListarBandejaAsync(CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        var rows = await conn.QueryAsync<CompraBandejaDto>(
            new CommandDefinition("logistica.usp_Bandeja_CompraDesdeRequerimiento_List",
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
        return rows.ToList();
    }
}
