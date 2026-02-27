namespace Chavez_Logistica.Dtos.Maestros.Proveedor;

public class ProveedorUpdateRequestDto
{
    public string? Ruc { get; set; }
    public string RazonSocial { get; set; } = null!;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; }
}