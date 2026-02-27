namespace Chavez_Logistica.Dtos.Maestros.Obras
{
    public class ObraUpdateRequestDto
    {
        public string Nombre { get; set; } = null!;
        public string? Ubicacion { get; set; }
        public bool Activa { get; set; }
    }
}
