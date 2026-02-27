namespace Chavez_Logistica.Dtos.Logistica.Requerimiento;

public class RequerimientoCambiarEstadoRequestDto
{
    public string Estado { get; set; } = null!;  // EJ: PENDIENTE / APROBADO / ANULADO
    public int? IdUsuario { get; set; }
    public string? Observacion { get; set; }
}