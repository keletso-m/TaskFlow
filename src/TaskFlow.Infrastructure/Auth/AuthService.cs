using Microsoft.AspNetCore.Identity;
using TaskFlow.Application.Auth;
using TaskFlow.Infrastructure.Identity;

namespace TaskFlow.Infrastructure.Auth;

internal class AuthService(UserManager<ApplicationUser> users, ITokenGenerator tokens) : IAuthService
{
    public async Task<AuthResult> RegisterAsync(RegisterRequest request)
    {
        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            DisplayName = request.DisplayName
        };

        var result = await users.CreateAsync(user, request.Password);
        return result.Succeeded
            ? AuthResult.Success(BuildResponse(user))
            : AuthResult.Fail(result.Errors.Select(e => e.Description).ToArray());
    }

    public async Task<AuthResult> LoginAsync(LoginRequest request)
    {
        var user = await users.FindByEmailAsync(request.Email);
        if (user is null || !await users.CheckPasswordAsync(user, request.Password))
            return AuthResult.Fail("Invalid email or password.");

        return AuthResult.Success(BuildResponse(user));
    }

    private AuthResponse BuildResponse(ApplicationUser user)
    {
        var (token, expires) = tokens.Generate(user.Id, user.Email!, user.DisplayName);
        return new AuthResponse(user.Id, user.Email!, user.DisplayName, token, expires);
    }
}
