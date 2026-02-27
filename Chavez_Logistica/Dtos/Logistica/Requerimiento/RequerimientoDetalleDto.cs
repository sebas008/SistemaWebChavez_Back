namespace Chavez_Logistica.Dtos.Logistica.Requerimiento;

public class RequerimientoDetalleDto
{
    public int? IdRequerimientoDetalle { get; set; }     // null al crear
    public int IdItem { get; set; }
    public decimal Cantidad { get; set; }
    public string? Observacion { get; set; }
}