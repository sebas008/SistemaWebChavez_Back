namespace Chavez_Logistica.Dtos.Logistica.Compra;

public class CompraDetalleDto
{
    public int? IdCompraDetalle { get; set; }
    public int IdItem { get; set; }
    public decimal Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public string? Observacion { get; set; }
}
