namespace Chavez_Logistica.Dtos.Maestros.Obras
{
    public class ObraDto
    {
        public int IdObra { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
