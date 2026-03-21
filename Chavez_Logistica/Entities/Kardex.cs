namespace Chavez_Logistica.Entities;

public class Kardex
{
    public DateTime Fecha { get; set; }
    public string? TipoMovimiento { get; set; }
    public int IdAlmacen { get; set; }
    public int IdItem { get; set; }
    public decimal Cantidad { get; set; }
    public string? Referencia { get; set; }
}
