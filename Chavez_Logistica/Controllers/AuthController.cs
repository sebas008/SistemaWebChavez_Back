using Microsoft.AspNetCore.Mvc;
using Chavez_Logistica.Dtos.Auth;
using Chavez_Logistica.Interfaces;

namespace Chavez_Logistica.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
        [FromBody] LoginRequestDto req,
        CancellationToken ct)
    {
        var result = await _service.LoginAsync(req, ct);
        if (result == null)
            return Unauthorized();

        return Ok(result);
    }
}