namespace Chavez_Logistica.Dtos.Usuarios
{
    public class UsuarioDto
    {
        public int IdUsuario { get; set; }
        public string UsuarioLogin { get; set; } = null!;
        public string Nombres { get; set; } = null!;
        public string? Email { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
