namespace Chavez_Logistica.Dtos.Logistica.RecepcionObra;

public class RecepcionObraDetalleDto
{
    public int? IdRecepcionObraDetalle { get; set; }
    public int IdOrdenFinalDetalle { get; set; }
    public int IdItem { get; set; }
    public decimal CantidadRecibida { get; set; }
    public string? Comentario { get; set; }
}
