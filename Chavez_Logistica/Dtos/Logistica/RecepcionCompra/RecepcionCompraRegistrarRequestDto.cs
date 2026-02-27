namespace Chavez_Logistica.Dtos.Logistica.RecepcionCompra;

public class RecepcionCompraRegistrarRequestDto
{
    public int IdCompra { get; set; }
    public int IdAlmacenDestino { get; set; }
    public string? Observacion { get; set; }
    public List<RecepcionCompraDetalleDto> Detalle { get; set; } = new();
    public int? IdUsuario { get; set; }
}
