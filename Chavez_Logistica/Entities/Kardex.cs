namespace Chavez_Logistica.Entities
{
    public class Kardex
    {

        public int IdKardex { get; set; }
        public DateTime Fecha { get; set; }
        public int IdAlmacen { get; set; }
        public int IdItem { get; set; }
        public string TipoMov { get; set; } = null!;
        public decimal Cantidad { get; set; }
        public string? Referencia { get; set; }
        public string? Observacion { get; set; }
        public int? IdUsuario { get; set; }
    }
}
