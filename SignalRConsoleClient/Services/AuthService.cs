using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using SignalRConsoleClient.Configuration;

namespace SignalRConsoleClient.Services;

public class AuthService
{
    private readonly AppConfig _config;

    public AuthService(IOptions<AppConfig> config)
    {
        _config = config.Value;
    }

    public async Task<string> GetTokenAsync()
    {
        var app = ConfidentialClientApplicationBuilder
            .Create(_config.ClientId)
            .WithClientSecret(_config.ClientSecret)
            .WithAuthority($"https://login.microsoftonline.com/{_config.TenantId}")
            .Build();

        var result = await app.AcquireTokenForClient([_config.Scope]).ExecuteAsync();

        return result.AccessToken;
    }
}