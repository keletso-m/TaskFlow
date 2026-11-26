namespace TaskFlow.Infrastructure.Auth;

public class JwtOptions
{
    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = "TaskFlow";
    public string Audience { get; set; } = "TaskFlow.Clients";
    public int ExpiryMinutes { get; set; } = 60;
}
