using System.Data;
using Dapper;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class RecepcionObraRepository : IRecepcionObraRepository
{
    private readonly IDbConnectionFactory _db;
    public RecepcionObraRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<RecepcionObra>> ListAsync(int? idOrdenFinal, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<RecepcionObra>(new CommandDefinition("logistica.usp_RecepcionObra_List",
            new { IdOrdenFinal = idOrdenFinal, Estado = estado }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<RecepcionObra?> GetByIdAsync(int idRecepcionObra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<RecepcionObra>(new CommandDefinition("logistica.usp_RecepcionObra_GetById",
            new { IdRecepcionObra = idRecepcionObra }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<IEnumerable<RecepcionObraDetalle>> Detalle_ListByRecepcionObraAsync(int idRecepcionObra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<RecepcionObraDetalle>(new CommandDefinition("logistica.usp_RecepcionObraDetalle_ListByRecepcionObra",
            new { IdRecepcionObra = idRecepcionObra }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<(int IdRecepcionObra, string Codigo)> RegistrarAsync(int idOrdenFinal, int idAlmacenOrigen, int idAlmacenDestino, string? observacion, int? idUsuario, IEnumerable<RecepcionObraDetalle> detalle, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        var dt = new DataTable();
        dt.Columns.Add("IdOrdenFinalDetalle", typeof(int));
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("CantidadRecibida", typeof(decimal));
        dt.Columns.Add("Comentario", typeof(string));

        foreach (var d in detalle)
        {
            var r = dt.NewRow();
            r["IdOrdenFinalDetalle"] = d.IdOrdenFinalDetalle;
            r["IdItem"] = d.IdItem;
            r["CantidadRecibida"] = d.CantidadRecibida;
            r["Comentario"] = (object?)d.Comentario ?? DBNull.Value;
            dt.Rows.Add(r);
        }

        var p = new DynamicParameters();
        p.Add("@IdOrdenFinal", idOrdenFinal);
        p.Add("@IdAlmacenOrigen", idAlmacenOrigen);
        p.Add("@IdAlmacenDestino", idAlmacenDestino);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", dt.AsTableValuedParameter("logistica.TVP_RecepcionObraDetalle"));
        p.Add("@IdRecepcionObra", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_RecepcionObra_Registrar", p, commandType: CommandType.StoredProcedure, cancellationToken: ct));
        return (p.Get<int>("@IdRecepcionObra"), p.Get<string>("@Codigo"));
    }
}
