using System.Data;
using Dapper;
using Chavez_Logistica.Entities;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Repositorys;

public class ProveedorRepository : IProveedorRepository
{
    private readonly IDbConnectionFactory _db;
    public ProveedorRepository(IDbConnectionFactory db) => _db = db;

    public async Task<IEnumerable<Proveedor>> ListAsync(bool? soloActivos, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryAsync<Proveedor>(new CommandDefinition(
            "maestros.usp_Proveedor_List",
            new { SoloActivos = soloActivos },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task<Proveedor?> GetByIdAsync(int idProveedor, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<Proveedor>(new CommandDefinition(
            "maestros.usp_Proveedor_GetById",
            new { IdProveedor = idProveedor },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task<int> CrearAsync(Proveedor entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        return await conn.QuerySingleAsync<int>(new CommandDefinition(
            "maestros.usp_Proveedor_Crear",
            new { entity.Ruc, entity.RazonSocial, entity.Email, entity.Telefono },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }

    public async Task ActualizarAsync(int idProveedor, Proveedor entity, CancellationToken ct)
    {
        using var conn = _db.CreateConnection();
        await conn.ExecuteAsync(new CommandDefinition(
            "maestros.usp_Proveedor_Actualizar",
            new { IdProveedor = idProveedor, entity.Ruc, entity.RazonSocial, entity.Email, entity.Telefono, entity.Activo },
            commandType: CommandType.StoredProcedure,
            cancellationToken: ct));
    }
}
