namespace Chavez_Logistica.Entities
{
    public class Item
    {
        public int IdItem { get; set; }
        public string? Partida { get; set; }
        public string Descripcion { get; set; } = null!;
        public int? IdUnidadMedida { get; set; }
        public bool Activo { get; set; }
    }
}
