namespace Chavez_Logistica.Dtos.Logistica.Atencion;

public class AtencionDetalleDto
{
    public int? IdAtencionDetalle { get; set; }
    public int IdItem { get; set; }
    public string? ItemNombre { get; set; }
    public decimal CantidadPlanificada { get; set; }
    public decimal CantidadAtendida { get; set; }
    public string? Estado { get; set; }
    public string? Comentario { get; set; }
    public string? Observacion { get; set; }
}
