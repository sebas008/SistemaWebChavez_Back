using System;

namespace Chavez_Logistica.Dtos.Logistica.RecepcionObra;

public class RecepcionObraDto
{
    public int IdRecepcionObra { get; set; }
    public string Codigo { get; set; } = null!;
    public int IdOrdenFinal { get; set; }
    public int IdAlmacenOrigen { get; set; }
    public int IdAlmacenDestino { get; set; }
    public DateTime FechaRecepcion { get; set; }
    public string Estado { get; set; } = null!;
    public string? Observacion { get; set; }
    public List<RecepcionObraDetalleDto> Detalle { get; set; } = new();
}
