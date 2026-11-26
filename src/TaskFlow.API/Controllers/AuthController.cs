using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.Auth;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await auth.RegisterAsync(request);
        return result.Succeeded ? Ok(result.Response) : BadRequest(new { errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await auth.LoginAsync(request);
        return result.Succeeded ? Ok(result.Response) : Unauthorized(new { errors = result.Errors });
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me() => Ok(new
    {
        id = User.FindFirst("sub")?.Value,
        email = User.FindFirst("email")?.Value,
        name = User.FindFirst("name")?.Value
    });
}
