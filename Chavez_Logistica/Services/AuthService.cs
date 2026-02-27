using Chavez_Logistica.Dtos.Auth;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repo;

    public AuthService(IAuthRepository repo)
    {
        _repo = repo;
    }

    public async Task<LoginResponseDto?> LoginAsync(
        LoginRequestDto req,
        CancellationToken ct)
    {
        return await _repo.LoginAsync(
            req.Usuario.Trim(),
            req.Password,
            ct);
    }
}