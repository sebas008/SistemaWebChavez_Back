namespace Chavez_Logistica.Dtos.Maestros.UnidadMedida
{
    public class UnidadMedidaUpdateRequestDto
    {
        public string? Codigo { get; set; }
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; } = true;
    }
}
