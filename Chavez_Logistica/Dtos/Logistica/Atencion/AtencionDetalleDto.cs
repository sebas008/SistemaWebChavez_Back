namespace Chavez_Logistica.Dtos.Logistica.Atencion;

public class AtencionDetalleDto
{
    public int? IdAtencionDetalle { get; set; }
    public int IdItem { get; set; }
    public decimal CantidadPlanificada { get; set; }
    public decimal CantidadAtendida { get; set; }
    public string? Comentario { get; set; }
}
