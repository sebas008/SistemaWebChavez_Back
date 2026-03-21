namespace Chavez_Logistica.Dtos.Logistica.Requerimiento;

public class RequerimientoDetalleCambiarEstadoRequestDto
{
    public string Estado { get; set; } = string.Empty;
    public bool? EntregaATiempo { get; set; }
    public int? IdUsuario { get; set; }
}
