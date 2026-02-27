namespace Chavez_Logistica.Entities
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }
        public string? Ruc { get; set; }
        public string RazonSocial { get; set; } = null!;
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public bool Activo { get; set; }
    }
}
