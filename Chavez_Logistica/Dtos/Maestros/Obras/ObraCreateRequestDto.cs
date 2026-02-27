namespace Chavez_Logistica.Dtos.Maestros.Obras
{
    public class ObraCreateRequestDto
    {
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public string? Ubicacion { get; set; }
    }
}
