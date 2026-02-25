namespace Chavez_Logistica.Dtos.Usuarios
{
    public class UsuarioCreateRequestDto
    {
        public string UsuarioLogin { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string? Email { get; set; }
    }
}
