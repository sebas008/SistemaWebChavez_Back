using System.Data;
using Dapper;
using Chavez_Logistica.Dtos.Logistica.Atencion;
using Chavez_Logistica.Entities.Logistica;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class AtencionRepository : IAtencionRepository
{
    private readonly IDbConnectionFactory _db;
    public AtencionRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Atencion>> ListAsync(int? idObra, int? idAlmacenOrigen, int? idAlmacenDestino, string? estado, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Atencion>(
            new CommandDefinition(
                "logistica.usp_Atencion_List",
                new { IdObra = idObra, IdAlmacenOrigen = idAlmacenOrigen, IdAlmacenDestino = idAlmacenDestino, Estado = estado },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<Atencion?> GetByIdAsync(int idAtencion, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Atencion>(
            new CommandDefinition(
                "logistica.usp_Atencion_GetById",
                new { IdAtencion = idAtencion },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<IEnumerable<AtencionDetalle>> Detalle_ListByAtencionAsync(int idAtencion, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<AtencionDetalle>(
            new CommandDefinition(
                "logistica.usp_AtencionDetalle_ListByAtencion",
                new { IdAtencion = idAtencion },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<List<AtencionBandejaDto>> ListarBandejaAsync(CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        var rows = await conn.QueryAsync<AtencionBandejaDto>(
            new CommandDefinition(
                "logistica.usp_Bandeja_AlmacenInternoDesdeRequerimiento_List",
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
        return rows.ToList();
    }

    public async Task<(int IdAtencion, string Codigo)> RegistrarDesdeAlmacenInternoAsync(
        int idObra,
        int idAlmacenOrigen,
        int idAlmacenDestino,
        string metodoAtencion,
        string? observacion,
        int? idUsuario,
        IEnumerable<AtencionDetalle> detalle,
        CancellationToken ct)
    {
        using var conn = _db.CreateConnection();

        var dt = new DataTable();
        dt.Columns.Add("IdItem", typeof(int));
        dt.Columns.Add("CantidadPlanificada", typeof(decimal));
        dt.Columns.Add("CantidadAtendida", typeof(decimal));
        dt.Columns.Add("Comentario", typeof(string));

        foreach (var d in detalle)
        {
            var row = dt.NewRow();
            row["IdItem"] = d.IdItem;
            row["CantidadPlanificada"] = d.CantidadPlanificada;
            row["CantidadAtendida"] = d.CantidadAtendida;
            row["Comentario"] = string.IsNullOrWhiteSpace(d.Comentario) ? DBNull.Value : d.Comentario;
            dt.Rows.Add(row);
        }

        var p = new DynamicParameters();
        p.Add("@IdObra", idObra);
        p.Add("@IdAlmacenOrigen", idAlmacenOrigen);
        p.Add("@IdAlmacenDestino", idAlmacenDestino);
        p.Add("@MetodoAtencion", metodoAtencion);
        p.Add("@Observacion", observacion);
        p.Add("@IdUsuario", idUsuario);
        p.Add("@Detalle", dt.AsTableValuedParameter("logistica.TVP_AtencionDetalle"));
        p.Add("@IdAtencion", dbType: DbType.Int32, direction: ParameterDirection.Output);
        p.Add("@Codigo", dbType: DbType.String, size: 30, direction: ParameterDirection.Output);

        await conn.ExecuteAsync(
            new CommandDefinition(
                "logistica.usp_Atencion_DesdeAlmacenInterno",
                p,
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );

        return (p.Get<int>("@IdAtencion"), p.Get<string>("@Codigo"));
    }
}
