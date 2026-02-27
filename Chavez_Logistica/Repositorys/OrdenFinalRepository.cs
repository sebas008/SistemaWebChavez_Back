using System.Data;
using Dapper;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class OrdenFinalRepository : IOrdenFinalRepository
{
    private readonly IDbConnectionFactory _db;
    public OrdenFinalRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<OrdenFinal>> ListAsync(int? idObra, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<OrdenFinal>(new CommandDefinition("logistica.usp_OrdenFinal_List",
            new { IdObra = idObra, Estado = estado }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<OrdenFinal?> GetByIdAsync(int idOrdenFinal, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<OrdenFinal>(new CommandDefinition("logistica.usp_OrdenFinal_GetById",
            new { IdOrdenFinal = idOrdenFinal }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<IEnumerable<OrdenFinalDetalle>> Detalle_ListByOrdenFinalAsync(int idOrdenFinal, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<OrdenFinalDetalle>(new CommandDefinition("logistica.usp_OrdenFinalDetalle_ListByOrdenFinal",
            new { IdOrdenFinal = idOrdenFinal }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<(int IdOrdenFinal, string Codigo)> CrearAsync(int idRequerimiento, string? observacion, int? idUsuario, IEnumerable<OrdenFinalDetalle> detalle, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        var dt = new DataTable();
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("Cantidad", typeof(decimal));
        dt.Columns.Add("Observacion", typeof(string));

        foreach (var d in detalle)
        {
            var row = dt.NewRow();
            row["IdItem"] = d.IdItem;
            row["Cantidad"] = d.Cantidad;
            row["Observacion"] = (object?)d.Observacion ?? DBNull.Value;
            dt.Rows.Add(row);
        }

        var p = new DynamicParameters();
        p.Add("@IdRequerimiento", idRequerimiento);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", dt.AsTableValuedParameter("logistica.TVP_OrdenFinalDetalle"));
        p.Add("@IdOrdenFinal", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_OrdenFinal_Crear", p,
            commandType: CommandType.StoredProcedure, cancellationToken: ct));

        return (p.Get<int>("@IdOrdenFinal"), p.Get<string>("@Codigo"));
    }

    public async Task CambiarEstadoAsync(int idOrdenFinal, string estado, int? idUsuario, string? observacion, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_OrdenFinal_CambiarEstado",
            new { IdOrdenFinal = idOrdenFinal, Estado = estado, IdUsuario = idUsuario, Observacion = observacion },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }
}
