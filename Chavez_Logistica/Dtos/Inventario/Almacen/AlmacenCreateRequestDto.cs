namespace Chavez_Logistica.Dtos.Inventario.Almacen
{
    public class AlmacenCreateRequestDto
    {
        public string Tipo { get; set; } = null!;  // INTERNO / OBRA
        public int? IdObra { get; set; }           // requerido si Tipo=OBRA
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
    }
}
