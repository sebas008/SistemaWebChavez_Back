namespace Chavez_Logistica.Entities
{
    public class Obra
    {
        public int IdObra { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public bool Activa { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
