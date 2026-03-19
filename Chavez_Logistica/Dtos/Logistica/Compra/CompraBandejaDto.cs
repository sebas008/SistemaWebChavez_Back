namespace Chavez_Logistica.Dtos.Logistica.Compra;
public class CompraBandejaDto
{
    public int IdRequerimientoDetalle { get; set; }
    public string NroReq { get; set; } = string.Empty;
    public string NomObra { get; set; } = string.Empty;
    public string Item { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public DateTime? Fecha { get; set; }
    public string Estado { get; set; } = string.Empty;
}
