using Chavez_Logistica.Dtos.Logistica.Compra;
namespace Chavez_Logistica.Interfaces;
public interface ICompraService
{
    Task<List<CompraBandejaDto>> ListarBandejaAsync(CancellationToken ct);
}
