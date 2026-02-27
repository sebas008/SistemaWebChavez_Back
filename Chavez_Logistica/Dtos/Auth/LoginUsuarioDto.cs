namespace Chavez_Logistica.Dtos.Auth
{
    public class LoginUsuarioDto
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string? Email { get; set; }
    }
}
