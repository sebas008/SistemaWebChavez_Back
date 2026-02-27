namespace Chavez_Logistica.Dtos.Auth
{
    public class LoginPermisoDto
    {
        public int IdPermiso { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
    }
}
