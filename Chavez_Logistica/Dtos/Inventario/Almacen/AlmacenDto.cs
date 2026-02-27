namespace Chavez_Logistica.Dtos.Inventario.Almacen
{
    public class AlmacenDto
    {

        public int IdAlmacen { get; set; }
        public string Tipo { get; set; } = null!;
        public int? IdObra { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }

    }
}
