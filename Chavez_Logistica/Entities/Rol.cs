namespace Chavez_Logistica.Entities
{
    public class Rol
    {
        public int IdRol { get; set; }
        public string Codigo { get; set; } = null!;
        public string Nombre { get; set; } = null!;
        public bool Activo { get; set; }

    }
}
