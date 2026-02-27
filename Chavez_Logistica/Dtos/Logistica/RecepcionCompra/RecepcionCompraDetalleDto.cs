namespace Chavez_Logistica.Dtos.Logistica.RecepcionCompra;

public class RecepcionCompraDetalleDto
{
    public int? IdRecepcionCompraDetalle { get; set; }
    public int IdCompraDetalle { get; set; }
    public int IdItem { get; set; }
    public decimal CantidadRecibida { get; set; }
    public string? Comentario { get; set; }
}
