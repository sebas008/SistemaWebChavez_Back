namespace Chavez_Logistica.Entities.Logistica;
public class RequerimientoDetalle
{
    public int IdRequerimientoDetalle { get; set; }
    public int IdRequerimiento { get; set; }
    public int IdItem { get; set; }
    public int? IdPartida { get; set; }
    public decimal Cantidad { get; set; }
    public int? IdUnidadMedida { get; set; }
    public string? Comentario { get; set; }
    public string? Observacion { get; set; }
    public string? Destino { get; set; }
    public string? EstadoItem { get; set; }
    public bool? EntregaATiempo { get; set; }
    public string? PartidaNombre { get; set; }
    public string? UnidadNombre { get; set; }
    public string? ItemNombre { get; set; }
}
