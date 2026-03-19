using Chavez_Logistica.Dtos.Logistica.Atencion;
namespace Chavez_Logistica.Interfaces;
public interface IAtencionRepository
{
    Task<List<AtencionBandejaDto>> ListarBandejaAsync(CancellationToken ct);
}
