using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalRConsoleClient.Configuration;
using SignalRConsoleClient.Models;
using SignalRConsoleClient.Utils;

namespace SignalRConsoleClient.Services;

public class SignalRService
{
    private readonly AppConfig _config;
    private readonly ILogger<SignalRService> _logger;
    private HubConnection? _connection;

    public SignalRService(IOptions<AppConfig> config, ILogger<SignalRService> logger)
    {
        _config = config.Value;
        _logger = logger;
    }

    public async Task ConnectAsync(string token)
    { 
        var url = _config.Environments[Environment.GetEnvironmentVariable("ENV") ?? "local"];

        _connection = new HubConnectionBuilder()
            .WithUrl(url, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(token);
            })
            .WithAutomaticReconnect()
            .Build();

        await _connection.StartAsync();
        _logger.LogInformation("Connected to VoxBridge SignalR hub.");
    }

    public async Task EchoAsync(string? message)
    {
        _connection?.On<string, string>("echo", (name, message) =>
        {
            Console.WriteLine($"Echo received from '{name}': {message}");
            Console.WriteLine();
        });

        await _connection!.InvokeAsync("Echo", "SignalR Console Client Test", message);
        _logger.LogInformation("Echo from VoxBridge SignalR Hub.");
    }

    public async Task SubscribeFlightsAsync(string? locationId)
    {
        _connection?.On<Flight>("ReceiveFlights", flight =>
        {
            JsonConsoleWriter.Write(flight);
        });

        await _connection!.InvokeAsync("SubscribeOnAirlineFlights", locationId ?? _config.LocationId);
    }

    public async Task SubscribeAnnouncementsAsync(string? locationId)
    {
        _connection?.On<Announcement[]>("ReceiveFlightAnnouncements", announcements =>
        {
            foreach (var announcement in announcements)
                JsonConsoleWriter.Write(announcement);
        });

        await _connection!.InvokeAsync("SubscribeOnAirlineFlightAnnouncements", locationId ?? _config.LocationId);
    }
}