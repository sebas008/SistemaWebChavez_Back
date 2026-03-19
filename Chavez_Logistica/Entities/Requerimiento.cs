namespace Chavez_Logistica.Entities.Logistica;

public class Requerimiento
{
    public int IdRequerimiento { get; set; }
    public string Codigo { get; set; } = null!;
    public int IdObra { get; set; }
    public int? IdUsuarioCreador { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = null!;
    public string? Observacion { get; set; }
    public bool Activo { get; set; }
    public bool? EntregaATiempo { get; set; }
}
