using Chavez_Logistica.Dtos.Auth;

namespace Chavez_Logistica.Interfaces
{
    public interface IAuthRepository
    {
        Task<LoginResponseDto?> LoginAsync(string usuario, string password, CancellationToken ct);
    }
}
