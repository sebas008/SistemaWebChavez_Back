using Chavez_Logistica.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDbConnectionFactory _db;

    public DashboardController(IDbConnectionFactory db)
    {
        _db = db;
    }

    public sealed class DashboardKpisDto
    {
        public int Proveedores { get; set; }
        public int Almacenes { get; set; }
        public int Usuarios { get; set; }
        public int Requerimientos { get; set; }
        public int Compras { get; set; }
        public int Atenciones { get; set; }
    }

    [HttpGet("kpis")]
    public ActionResult<DashboardKpisDto> GetKpis()
    {
        // IMPORTANTE:
        // - No dependemos de SPs (porque a veces no están o cambian).
        // - Contamos de forma defensiva: si una tabla no existe o no tiene Activo, devolvemos 0.

        const string sql = @"
DECLARE @Proveedores INT = 0,
        @Almacenes INT = 0,
        @Usuarios INT = 0,
        @Requerimientos INT = 0,
        @Compras INT = 0,
        @Atenciones INT = 0;

-- maestros.Proveedor
IF OBJECT_ID('maestros.Proveedor','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('maestros.Proveedor','Activo') IS NOT NULL
        SELECT @Proveedores = COUNT(1) FROM maestros.Proveedor WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Proveedores = COUNT(1) FROM maestros.Proveedor WITH (NOLOCK);
END

-- inventario.Almacen
IF OBJECT_ID('inventario.Almacen','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('inventario.Almacen','Activo') IS NOT NULL
        SELECT @Almacenes = COUNT(1) FROM inventario.Almacen WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Almacenes = COUNT(1) FROM inventario.Almacen WITH (NOLOCK);
END

-- seguridad.Usuario
IF OBJECT_ID('seguridad.Usuario','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('seguridad.Usuario','Activo') IS NOT NULL
        SELECT @Usuarios = COUNT(1) FROM seguridad.Usuario WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Usuarios = COUNT(1) FROM seguridad.Usuario WITH (NOLOCK);
END

-- logistica.Requerimiento
IF OBJECT_ID('logistica.Requerimiento','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('logistica.Requerimiento','Activo') IS NOT NULL
        SELECT @Requerimientos = COUNT(1) FROM logistica.Requerimiento WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Requerimientos = COUNT(1) FROM logistica.Requerimiento WITH (NOLOCK);
END

-- logistica.Compra (si tu tabla se llama distinto, aquí se ajusta)
IF OBJECT_ID('logistica.Compra','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('logistica.Compra','Activo') IS NOT NULL
        SELECT @Compras = COUNT(1) FROM logistica.Compra WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Compras = COUNT(1) FROM logistica.Compra WITH (NOLOCK);
END
ELSE IF OBJECT_ID('logistica.OrdenCompra','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('logistica.OrdenCompra','Activo') IS NOT NULL
        SELECT @Compras = COUNT(1) FROM logistica.OrdenCompra WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Compras = COUNT(1) FROM logistica.OrdenCompra WITH (NOLOCK);
END

-- logistica.Atencion
IF OBJECT_ID('logistica.Atencion','U') IS NOT NULL
BEGIN
    IF COL_LENGTH('logistica.Atencion','Activo') IS NOT NULL
        SELECT @Atenciones = COUNT(1) FROM logistica.Atencion WITH (NOLOCK) WHERE Activo = 1;
    ELSE
        SELECT @Atenciones = COUNT(1) FROM logistica.Atencion WITH (NOLOCK);
END

SELECT
    @Proveedores AS Proveedores,
    @Almacenes AS Almacenes,
    @Usuarios AS Usuarios,
    @Requerimientos AS Requerimientos,
    @Compras AS Compras,
    @Atenciones AS Atenciones;
";

        using var cn = _db.CreateConnection();
        var kpis = cn.QuerySingle<DashboardKpisDto>(sql);
        return Ok(kpis);
    }
}
