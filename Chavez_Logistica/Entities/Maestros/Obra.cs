namespace Chavez_Logistica.Entities.Maestros;

public class Obra
{
    public int IdObra { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string? Ubicacion { get; set; }
    public bool Activa { get; set; }
}
