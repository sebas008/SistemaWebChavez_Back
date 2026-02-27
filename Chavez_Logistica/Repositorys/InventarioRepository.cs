using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;
using Dapper;
using System.Data;

namespace Chavez_Logistica.Repositorys;

public class InventarioRepository : IInventarioRepository
{
    private readonly IDbConnectionFactory _db;
    public InventarioRepository(IDbConnectionFactory db) => _db = db;

    // ---------- ALMACEN ----------
    public async Task<IEnumerable<Almacen>> Almacen_ListAsync(bool? soloActivos, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Almacen>(
            new CommandDefinition(
                "inventario.usp_Almacen_List",
                new { SoloActivos = soloActivos },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<Almacen?> Almacen_GetByIdAsync(int idAlmacen, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Almacen>(
            new CommandDefinition(
                "inventario.usp_Almacen_GetById",
                new { IdAlmacen = idAlmacen },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<int> Almacen_CrearAsync(Almacen entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                "inventario.usp_Almacen_Crear",
                new { entity.Tipo, entity.IdObra, entity.Codigo, entity.Nombre },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task Almacen_ActualizarAsync(int idAlmacen, string nombre, bool activo, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "inventario.usp_Almacen_Actualizar",
                new { IdAlmacen = idAlmacen, Nombre = nombre, Activo = activo },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    // ---------- ITEM ----------
    public async Task<IEnumerable<Item>> Item_ListAsync(bool? soloActivos, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Item>(
            new CommandDefinition(
                "inventario.usp_Item_List",
                new { SoloActivos = soloActivos },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<Item?> Item_GetByIdAsync(int idItem, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Item>(
            new CommandDefinition(
                "inventario.usp_Item_GetById",
                new { IdItem = idItem },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task<int> Item_CrearAsync(Item entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(
            new CommandDefinition(
                "inventario.usp_Item_Crear",
                new { entity.Partida, entity.Descripcion, entity.IdUnidadMedida },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    public async Task Item_ActualizarAsync(int idItem, Item entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(
            new CommandDefinition(
                "inventario.usp_Item_Actualizar",
                new
                {
                    IdItem = idItem,
                    entity.Partida,
                    entity.Descripcion,
                    entity.IdUnidadMedida,
                    entity.Activo
                },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    // ---------- STOCK ----------
    public async Task<IEnumerable<Stock>> Stock_ListAsync(int? idAlmacen, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Stock>(
            new CommandDefinition(
                "inventario.usp_Stock_List",
                new { IdAlmacen = idAlmacen },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }

    // ---------- KARDEX ----------
    public async Task<IEnumerable<Kardex>> Kardex_ListAsync(int? idAlmacen, int? idItem, DateTime? desde, DateTime? hasta, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Kardex>(
            new CommandDefinition(
                "inventario.usp_Kardex_List",
                new { IdAlmacen = idAlmacen, IdItem = idItem, Desde = desde, Hasta = hasta },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            )
        );
    }
}