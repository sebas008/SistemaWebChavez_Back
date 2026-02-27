namespace Chavez_Logistica.Dtos.Inventario.Item
{
    public class ItemCreateRequestDto
    {
        public string? Partida { get; set; }
        public string Descripcion { get; set; } = null!;
        public int? IdUnidadMedida { get; set; }
    }
}
