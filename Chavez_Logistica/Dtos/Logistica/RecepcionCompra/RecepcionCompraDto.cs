using System;

namespace Chavez_Logistica.Dtos.Logistica.RecepcionCompra;

public class RecepcionCompraDto
{
    public int IdRecepcionCompra { get; set; }
    public string Codigo { get; set; } = null!;
    public int IdCompra { get; set; }
    public int IdAlmacenDestino { get; set; }
    public DateTime FechaRecepcion { get; set; }
    public string Estado { get; set; } = null!;
    public string? Observacion { get; set; }
    public List<RecepcionCompraDetalleDto> Detalle { get; set; } = new();
}
