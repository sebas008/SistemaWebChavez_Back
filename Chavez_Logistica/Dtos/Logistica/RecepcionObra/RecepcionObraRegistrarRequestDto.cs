namespace Chavez_Logistica.Dtos.Logistica.RecepcionObra;

public class RecepcionObraRegistrarRequestDto
{
    public int IdOrdenFinal { get; set; }
    public int IdAlmacenOrigen { get; set; }
    public int IdAlmacenDestino { get; set; }
    public string? Observacion { get; set; }
    public List<RecepcionObraDetalleDto> Detalle { get; set; } = new();
    public int? IdUsuario { get; set; }
}
