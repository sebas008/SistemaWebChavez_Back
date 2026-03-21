using System.Data;
using Dapper;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chavez_Logistica.Repositorys;

public class RequerimientoRepository : IRequerimientoRepository
{
    private readonly IDbConnectionFactory _db;
    public RequerimientoRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Requerimiento>> ListAsync(int? idObra, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Requerimiento>(
            new CommandDefinition(
                "logistica.usp_Requerimiento_List",
                new { IdObra = idObra, Estado = estado },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<Requerimiento?> GetByIdAsync(int idRequerimiento, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Requerimiento>(
            new CommandDefinition(
                "logistica.usp_Requerimiento_GetById",
                new { IdRequerimiento = idRequerimiento },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<IEnumerable<RequerimientoDetalle>> Detalle_ListByRequerimientoAsync(int idRequerimiento, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<RequerimientoDetalle>(
            new CommandDefinition(
                "logistica.usp_RequerimientoDetalle_ListByRequerimiento",
                new { IdRequerimiento = idRequerimiento },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<(int IdRequerimiento, string Codigo)> CrearAsync(
        int idObra,
        string? observacion,
        int? idUsuario,
        IEnumerable<RequerimientoDetalle> detalle,
        CancellationToken ct)
    {
        var detalleList = detalle?.ToList() ?? new List<RequerimientoDetalle>();
        using var conn = _db.CreateConnection();

        try
        {
            return await EjecutarCrearAsync(conn, idObra, observacion, idUsuario, CrearDetalleTableV2(detalleList), "logistica.TVP_RequerimientoDetalle_V2", ct);
        }
        catch (SqlException ex) when (DebeUsarTvpLegado(ex))
        {
            return await EjecutarCrearAsync(conn, idObra, observacion, idUsuario, CrearDetalleTableLegado(detalleList), "logistica.TVP_RequerimientoDetalle", ct);
        }
    }

    public async Task CambiarEstadoAsync(
        int idRequerimiento,
        string estado,
        int? idUsuario,
        string? observacion,
        CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "logistica.usp_Requerimiento_CambiarEstado",
                new
                {
                    IdRequerimiento = idRequerimiento,
                    Estado = estado,
                    IdUsuario = idUsuario,
                    Observacion = observacion
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task AsignarDestinoDetalleAsync(
        int idRequerimientoDetalle,
        string? destino,
        int? idUsuario,
        CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "logistica.usp_RequerimientoDetalle_ActualizarDestino",
                new
                {
                    IdRequerimientoDetalle = idRequerimientoDetalle,
                    Destino = destino,
                    IdUsuario = idUsuario
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task CambiarEstadoDetalleAsync(
        int idRequerimientoDetalle,
        string estado,
        bool? entregaATiempo,
        CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "logistica.usp_RequerimientoDetalle_CambiarEstado",
                new
                {
                    IdRequerimientoDetalle = idRequerimientoDetalle,
                    Estado = estado,
                    EntregaATiempo = entregaATiempo
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    private static async Task<(int IdRequerimiento, string Codigo)> EjecutarCrearAsync(
        System.Data.IDbConnection conn,
        int idObra,
        string? observacion,
        int? idUsuario,
        DataTable detalle,
        string tvpName,
        CancellationToken ct)
    {
        var p = new DynamicParameters();
        p.Add("@IdObra", idObra);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", detalle.AsTableValuedParameter(tvpName));
        p.Add("@IdRequerimiento", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(
            new CommandDefinition(
                "logistica.usp_Requerimiento_Crear",
                p,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );

        return (p.Get<int>("@IdRequerimiento"), p.Get<string>("@Codigo"));
    }

    private static DataTable CrearDetalleTableV2(IEnumerable<RequerimientoDetalle> detalle)
    {
        var dt = new DataTable();
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("IdPartida", typeof(int));
        dt.Columns.Add("Cantidad", typeof(decimal));
        dt.Columns.Add("IdUnidadMedida", typeof(int));
        dt.Columns.Add("Comentario", typeof(string));
        dt.Columns.Add("Observacion", typeof(string));

        foreach (var d in detalle)
        {
            var row = dt.NewRow();
            row["IdItem"] = d.IdItem;
            row["IdPartida"] = d.IdPartida.HasValue ? d.IdPartida.Value : DBNull.Value;
            row["Cantidad"] = d.Cantidad;
            row["IdUnidadMedida"] = d.IdUnidadMedida.HasValue ? d.IdUnidadMedida.Value : DBNull.Value;
            row["Comentario"] = string.IsNullOrWhiteSpace(d.Comentario) ? DBNull.Value : d.Comentario;
            row["Observacion"] = string.IsNullOrWhiteSpace(d.Observacion) ? DBNull.Value : d.Observacion;
            dt.Rows.Add(row);
        }

        return dt;
    }

    private static DataTable CrearDetalleTableLegado(IEnumerable<RequerimientoDetalle> detalle)
    {
        var dt = new DataTable();
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("Cantidad", typeof(decimal));
        dt.Columns.Add("Observacion", typeof(string));

        foreach (var d in detalle)
        {
            var row = dt.NewRow();
            row["IdItem"] = d.IdItem;
            row["Cantidad"] = d.Cantidad;
            row["Observacion"] = string.IsNullOrWhiteSpace(d.Observacion)
                ? (string.IsNullOrWhiteSpace(d.Comentario) ? DBNull.Value : d.Comentario)
                : d.Observacion;
            dt.Rows.Add(row);
        }

        return dt;
    }

    private static bool DebeUsarTvpLegado(SqlException ex)
    {
        var msg = ex.Message ?? string.Empty;
        return msg.Contains("table-valued parameter", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("TVP_RequerimientoDetalle", StringComparison.OrdinalIgnoreCase)
            || msg.Contains("requires 3 column", StringComparison.OrdinalIgnoreCase);
    }
}
