namespace Chavez_Logistica.Dtos.Logistica.Compra;

public class CompraCreateRequestDto
{
    public int IdProveedor { get; set; }
    public int IdObra { get; set; }
    public string? Observacion { get; set; }
    public List<CompraDetalleDto> Detalle { get; set; } = new();
    public int? IdUsuario { get; set; }
}
