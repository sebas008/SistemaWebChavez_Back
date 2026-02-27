using System.Data;
using Dapper;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class RecepcionCompraRepository : IRecepcionCompraRepository
{
    private readonly IDbConnectionFactory _db;
    public RecepcionCompraRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<RecepcionCompra>> ListAsync(int? idCompra, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<RecepcionCompra>(new CommandDefinition("logistica.usp_RecepcionCompra_List",
            new { IdCompra = idCompra, Estado = estado }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<RecepcionCompra?> GetByIdAsync(int idRecepcionCompra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<RecepcionCompra>(new CommandDefinition("logistica.usp_RecepcionCompra_GetById",
            new { IdRecepcionCompra = idRecepcionCompra }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<IEnumerable<RecepcionCompraDetalle>> Detalle_ListByRecepcionCompraAsync(int idRecepcionCompra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<RecepcionCompraDetalle>(new CommandDefinition("logistica.usp_RecepcionCompraDetalle_ListByRecepcionCompra",
            new { IdRecepcionCompra = idRecepcionCompra }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<(int IdRecepcionCompra, string Codigo)> RegistrarAsync(int idCompra, int idAlmacenDestino, string? observacion, int? idUsuario, IEnumerable<RecepcionCompraDetalle> detalle, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        var dt = new DataTable();
        dt.Columns.Add("IdCompraDetalle", typeof(int));
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("CantidadRecibida", typeof(decimal));
        dt.Columns.Add("Comentario", typeof(string));

        foreach (var d in detalle)
        {
            var r = dt.NewRow();
            r["IdCompraDetalle"] = d.IdCompraDetalle;
            r["IdItem"] = d.IdItem;
            r["CantidadRecibida"] = d.CantidadRecibida;
            r["Comentario"] = (object?)d.Comentario ?? DBNull.Value;
            dt.Rows.Add(r);
        }

        var p = new DynamicParameters();
        p.Add("@IdCompra", idCompra);
        p.Add("@IdAlmacenDestino", idAlmacenDestino);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", dt.AsTableValuedParameter("logistica.TVP_RecepcionCompraDetalle"));
        p.Add("@IdRecepcionCompra", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_RecepcionCompra_Registrar", p, commandType: CommandType.StoredProcedure, cancellationToken: ct));
        return (p.Get<int>("@IdRecepcionCompra"), p.Get<string>("@Codigo"));
    }
}
