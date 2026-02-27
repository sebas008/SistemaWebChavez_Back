namespace Chavez_Logistica.Dtos.Logistica.Compra;

public class CompraCambiarEstadoRequestDto
{
    public string Estado { get; set; } = null!;
    public int? IdUsuario { get; set; }
    public string? Observacion { get; set; }
}
