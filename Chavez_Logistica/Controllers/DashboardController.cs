using Chavez_Logistica.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly IDbConnectionFactory _db;
    public DashboardController(IDbConnectionFactory db) => _db = db;

    public sealed class DashboardKpisDto
    {
        public int Proveedores { get; set; }
        public int Almacenes { get; set; }
        public int Usuarios { get; set; }
        public int Requerimientos { get; set; }
        public int Compras { get; set; }
        public int Atenciones { get; set; }
        public decimal PorcentajeEntregaCompleta { get; set; }
        public decimal PorcentajeEntregaATiempo { get; set; }
        public decimal PorcentajeCompra { get; set; }
        public decimal PorcentajeAlmacenInterno { get; set; }
    }

    [HttpGet("kpis")]
    public ActionResult<DashboardKpisDto> GetKpis()
    {
        using var cn = _db.CreateConnection();
        var row = cn.QuerySingle<DashboardKpisDto>("logistica.usp_Dashboard_Kpis", commandType: CommandType.StoredProcedure);
        return Ok(row);
    }
}
