namespace Chavez_Logistica.Dtos.Inventario.Item
{
    public class ItemDto
    {
        public int IdItem { get; set; }
        public string? Partida { get; set; }
        public string Descripcion { get; set; } = null!;
        public int? IdUnidadMedida { get; set; }
        public bool Activo { get; set; }
    }
}
