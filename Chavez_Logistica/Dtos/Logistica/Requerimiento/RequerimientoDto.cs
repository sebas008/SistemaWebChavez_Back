using System;

namespace Chavez_Logistica.Dtos.Logistica.Requerimiento;

public class RequerimientoDto
{
    public int IdRequerimiento { get; set; }
    public string Codigo { get; set; } = null!;
    public int IdObra { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = null!;
    public string? Observacion { get; set; }

    public List<RequerimientoDetalleDto> Detalle { get; set; } = new();
}