using System;

namespace Chavez_Logistica.Dtos.Logistica.Atencion;

public class AtencionDto
{
    public int IdAtencion { get; set; }
    public string Codigo { get; set; } = null!;
    public DateTime Fecha { get; set; }
    public int IdObra { get; set; }
    public int IdAlmacenOrigen { get; set; }
    public int IdAlmacenDestino { get; set; }
    public string MetodoAtencion { get; set; } = null!;
    public string Estado { get; set; } = null!;
    public string? Observacion { get; set; }
    public List<AtencionDetalleDto> Detalle { get; set; } = new();
}
