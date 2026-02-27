using System.Data;
using Dapper;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class CompraRepository : ICompraRepository
{
    private readonly IDbConnectionFactory _db;
    public CompraRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Compra>> ListAsync(int? idProveedor, int? idObra, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Compra>(new CommandDefinition("logistica.usp_Compra_List",
            new { IdProveedor = idProveedor, IdObra = idObra, Estado = estado }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<Compra?> GetByIdAsync(int idCompra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Compra>(new CommandDefinition("logistica.usp_Compra_GetById",
            new { IdCompra = idCompra }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<IEnumerable<CompraDetalle>> Detalle_ListByCompraAsync(int idCompra, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<CompraDetalle>(new CommandDefinition("logistica.usp_CompraDetalle_ListByCompra",
            new { IdCompra = idCompra }, commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }

    public async Task<(int IdCompra, string Codigo)> CrearAsync(int idProveedor, int idObra, string? observacion, int? idUsuario, IEnumerable<CompraDetalle> detalle, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        var dt = new DataTable();
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("Cantidad", typeof(decimal));
        dt.Columns.Add("PrecioUnitario", typeof(decimal));
        dt.Columns.Add("Observacion", typeof(string));

        foreach (var d in detalle)
        {
            var r = dt.NewRow();
            r["IdItem"] = d.IdItem;
            r["Cantidad"] = d.Cantidad;
            r["PrecioUnitario"] = d.PrecioUnitario;
            r["Observacion"] = (object?)d.Observacion ?? DBNull.Value;
            dt.Rows.Add(r);
        }

        var p = new DynamicParameters();
        p.Add("@IdProveedor", idProveedor);
        p.Add("@IdObra", idObra);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", dt.AsTableValuedParameter("logistica.TVP_CompraDetalle"));
        p.Add("@IdCompra", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_Compra_Crear", p, commandType: CommandType.StoredProcedure, cancellationToken: ct));
        return (p.Get<int>("@IdCompra"), p.Get<string>("@Codigo"));
    }

    public async Task CambiarEstadoAsync(int idCompra, string estado, int? idUsuario, string? observacion, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition("logistica.usp_Compra_CambiarEstado",
            new { IdCompra = idCompra, Estado = estado, IdUsuario = idUsuario, Observacion = observacion },
            commandType: CommandType.StoredProcedure, cancellationToken: ct));
    }
}
