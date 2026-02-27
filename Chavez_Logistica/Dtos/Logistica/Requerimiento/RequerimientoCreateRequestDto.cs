namespace Chavez_Logistica.Dtos.Logistica.Requerimiento;

public class RequerimientoCreateRequestDto
{
    public int IdObra { get; set; }
    public string? Observacion { get; set; }
    public List<RequerimientoDetalleDto> Detalle { get; set; } = new();
    public int? IdUsuario { get; set; } // si quieres auditar
}