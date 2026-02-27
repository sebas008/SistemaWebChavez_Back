using System.Data;
using Dapper;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class RequerimientoRepository : IRequerimientoRepository
{
    private readonly IDbConnectionFactory _db;
    public RequerimientoRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Requerimiento>> ListAsync(int? idObra, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Requerimiento>(new CommandDefinition("logistica.usp_Requerimiento_List",
            new { IdObra = idObra, Estado = estado }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<Requerimiento?> GetByIdAsync(int idRequerimiento, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Requerimiento>(new CommandDefinition("logistica.usp_Requerimiento_GetById",
            new { IdRequerimiento = idRequerimiento }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<IEnumerable<RequerimientoDetalle>> Detalle_ListByRequerimientoAsync(int idRequerimiento, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<RequerimientoDetalle>(new CommandDefinition("logistica.usp_RequerimientoDetalle_ListByRequerimiento",
            new { IdRequerimiento = idRequerimiento }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<(int IdRequerimiento, string Codigo)> CrearAsync(int idObra, string? observacion, int? idUsuario, IEnumerable<RequerimientoDetalle> detalle, CancellationToken ct)
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
        p.Add("@IdObra", idObra);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", dt.AsTableValuedParameter("logistica.TVP_RequerimientoDetalle"));
        p.Add("@IdRequerimiento", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_Requerimiento_Crear", p,
            commandType: CommandType.StoredProcedure, cancellationToken: ct));

        return (p.Get<int>("@IdRequerimiento"), p.Get<string>("@Codigo"));
    }

    public async Task CambiarEstadoAsync(int idRequerimiento, string estado, int? idUsuario, string? observacion, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_Requerimiento_CambiarEstado",
            new { IdRequerimiento = idRequerimiento, Estado = estado, IdUsuario = idUsuario, Observacion = observacion },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }
}
