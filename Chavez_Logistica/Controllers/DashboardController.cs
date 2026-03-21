using Chavez_Logistica.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDbConnectionFactory _db;
    public DashboardController(IDbConnectionFactory db) { _db = db; }

    public sealed class DashboardKpisDto
    {
        public int Proveedores { get; set; }
        public int Almacenes { get; set; }
        public int Usuarios { get; set; }
        public int Requerimientos { get; set; }
        public int Compras { get; set; }
        public int Atenciones { get; set; }
    }

    public sealed class DashboardIndicadoresDto
    {
        public decimal EntregaCompleta { get; set; }
        public decimal EntregaATiempo { get; set; }
        public decimal Compra { get; set; }
        public decimal AlmacenInterno { get; set; }
    }

    [HttpGet("kpis")]
    public ActionResult<DashboardKpisDto> GetKpis()
    {
        const string sql = @"
DECLARE @Proveedores INT = (SELECT COUNT(1) FROM maestros.Proveedor WHERE Activo = 1);
DECLARE @Almacenes INT = (SELECT COUNT(1) FROM inventario.Almacen WHERE Activo = 1);
DECLARE @Usuarios INT = (SELECT COUNT(1) FROM seguridad.Usuario WHERE Activo = 1);
DECLARE @Requerimientos INT = (SELECT COUNT(1) FROM logistica.Requerimiento WHERE Activo = 1);
DECLARE @Compras INT = (SELECT COUNT(1) FROM logistica.Compra WHERE Activo = 1);
DECLARE @Atenciones INT = (SELECT COUNT(1) FROM logistica.Atencion WHERE Activo = 1);
SELECT @Proveedores Proveedores,@Almacenes Almacenes,@Usuarios Usuarios,@Requerimientos Requerimientos,@Compras Compras,@Atenciones Atenciones;";
        using var cn = _db.CreateConnection();
        return Ok(cn.QuerySingle<DashboardKpisDto>(sql));
    }

    [HttpGet("indicadores-requerimientos")]
    public ActionResult<DashboardIndicadoresDto> GetIndicadores()
    {
        using var cn = _db.CreateConnection();
        var row = cn.QuerySingle<DashboardIndicadoresDto>("logistica.usp_Dashboard_IndicadoresRequerimientos", commandType: System.Data.CommandType.StoredProcedure);
        return Ok(row);
    }
}
