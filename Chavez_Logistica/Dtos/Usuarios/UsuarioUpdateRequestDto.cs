namespace Chavez_Logistica.Dtos.Usuarios
{
    public class UsuarioUpdateRequestDto
    {
        public string Nombres { get; set; } = null!;
        public string? Email { get; set; }
        public bool Activo { get; set; }
    }
}
