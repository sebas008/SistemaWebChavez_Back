using Chavez_Logistica.Dtos.Logistica.Atencion;
using Chavez_Logistica.Interfaces;
namespace Chavez_Logistica.Services;
public class AtencionService : IAtencionService
{
    private readonly IAtencionRepository _repo;
    public AtencionService(IAtencionRepository repo) => _repo = repo;
    public Task<List<AtencionBandejaDto>> ListarBandejaAsync(CancellationToken ct) => _repo.ListarBandejaAsync(ct);
}
