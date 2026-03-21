namespace Chavez_Logistica.Dtos.Logistica.Requerimiento;

public class RequerimientoDetalleDto
{
    public int? IdRequerimientoDetalle { get; set; }
    public int IdItem { get; set; }
    public int? IdPartida { get; set; }
    public int? IdUnidadMedida { get; set; }
    public decimal Cantidad { get; set; }
    public string? Comentario { get; set; }
    public string? Observacion { get; set; }
    public string? Destino { get; set; }
}
