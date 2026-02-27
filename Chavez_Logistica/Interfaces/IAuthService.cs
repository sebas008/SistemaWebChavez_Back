using Chavez_Logistica.Dtos.Auth;

namespace Chavez_Logistica.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto req,CancellationToken ct);
}