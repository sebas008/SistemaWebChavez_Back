namespace Chavez_Logistica.Dtos.Logistica.Atencion;

public class AtencionRegistrarRequestDto
{
    public int IdObra { get; set; }
    public int IdAlmacenOrigen { get; set; }
    public int IdAlmacenDestino { get; set; }
    public string MetodoAtencion { get; set; } = null!;
    public string? Observacion { get; set; }
    public List<AtencionDetalleDto> Detalle { get; set; } = new();
    public int? IdUsuario { get; set; }
}
