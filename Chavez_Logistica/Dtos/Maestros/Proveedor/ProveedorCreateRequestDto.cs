namespace Chavez_Logistica.Dtos.Maestros.Proveedor;

public class ProveedorCreateRequestDto
{
    public string? Ruc { get; set; }
    public string RazonSocial { get; set; } = null!;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
}