namespace Chavez_Logistica.Entities.Logistica;

public class AtencionDetalle
{
    public int IdAtencionDetalle { get; set; }
    public int IdAtencion { get; set; }
    public int IdItem { get; set; }
    public string? ItemNombre { get; set; }
    public decimal CantidadPlanificada { get; set; }
    public decimal CantidadAtendida { get; set; }
    public string? Estado { get; set; }
    public string? Comentario { get; set; }
    public string? Observacion { get; set; }
}
