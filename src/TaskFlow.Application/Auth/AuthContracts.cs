namespace TaskFlow.Application.Auth;

public record RegisterRequest(string Email, string Password, string DisplayName);
public record LoginRequest(string Email, string Password);
public record AuthResponse(Guid UserId, string Email, string DisplayName, string Token, DateTime ExpiresAt);
public record AuthResult(bool Succeeded, AuthResponse? Response, IReadOnlyList<string> Errors)
{
    public static AuthResult Success(AuthResponse r) => new(true, r, []);
    public static AuthResult Fail(params string[] errors) => new(false, null, errors);
}

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterRequest request);
    Task<AuthResult> LoginAsync(LoginRequest request);
}

public interface ITokenGenerator
{
    (string Token, DateTime ExpiresAt) Generate(Guid userId, string email, string displayName);
}
