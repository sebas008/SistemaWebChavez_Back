namespace Chavez_Logistica.Entities
{
    public class Almacen
    {
        public int IdAlmacen { get; set; }
        public string Tipo { get; set; } = null!;   // INTERNO / OBRA
        public int? IdObra { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }
    }
}
