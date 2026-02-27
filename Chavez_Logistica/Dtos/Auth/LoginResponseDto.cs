namespace Chavez_Logistica.Dtos.Auth
{
    public class LoginResponseDto
    {
        public LoginUsuarioDto Usuario { get; set; } = null!;
        public List<LoginRolDto> Roles { get; set; } = new();
        public List<LoginPermisoDto> Permisos { get; set; } = new();
    }
}
