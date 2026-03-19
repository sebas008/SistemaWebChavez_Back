using Chavez_Logistica.Dtos.Logistica.Compra;
using Chavez_Logistica.Interfaces;
namespace Chavez_Logistica.Services;
public class CompraService : ICompraService
{
    private readonly ICompraRepository _repo;
    public CompraService(ICompraRepository repo) => _repo = repo;
    public Task<List<CompraBandejaDto>> ListarBandejaAsync(CancellationToken ct) => _repo.ListarBandejaAsync(ct);
}
