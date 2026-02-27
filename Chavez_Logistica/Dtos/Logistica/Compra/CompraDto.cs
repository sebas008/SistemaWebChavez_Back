using System;

namespace Chavez_Logistica.Dtos.Logistica.Compra;

public class CompraDto
{
    public int IdCompra { get; set; }
    public string Codigo { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public int IdProveedor { get; set; }
    public int IdObra { get; set; }
    public string Estado { get; set; } = null!;
    public string? Observacion { get; set; }
    public List<CompraDetalleDto> Detalle { get; set; } = new();
}
