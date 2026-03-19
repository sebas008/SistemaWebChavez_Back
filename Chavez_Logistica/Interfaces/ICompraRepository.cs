using Chavez_Logistica.Dtos.Logistica.Compra;
namespace Chavez_Logistica.Interfaces;
public interface ICompraRepository
{
    Task<List<CompraBandejaDto>> ListarBandejaAsync(CancellationToken ct);
}
