using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalRConsoleClient.Configuration;
using SignalRConsoleClient.Models;
using SignalRConsoleClient.Utils;

namespace SignalRConsoleClient.Services;

public class SignalRService(IOptions<AppConfig> config, ILogger<SignalRService> logger)
{
    private readonly AppConfig _config = config.Value;
    private readonly ILogger<SignalRService> _logger = logger;
    private HubConnection? _connection;

    public HubConnectionState? State => _connection?.State;

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

        await _connection!.InvokeAsync("SubscribeToAirlineFlights", locationId ?? _config.LocationId);
    }

    public async Task UnsubscribeFlightsAsync(string? locationId)
    {
        if (_connection == null)
            return;

        await _connection!.InvokeAsync("UnsubscribeFromAirlineFlights", locationId ?? _config.LocationId);
    }

    public async Task SubscribeFlightAnnouncementsAsync(string? locationId)
    {
        _connection?.On<Announcement>("ReceiveFlightAnnouncements", announcement =>
        {
            JsonConsoleWriter.Write(announcement);
        });

        await _connection!.InvokeAsync("SubscribeToAirlineFlightAnnouncements", locationId ?? _config.LocationId);
    }

    public async Task UnsubscribeFlightAnnouncementsAsync(string? locationId)
    {
        if (_connection == null)
            return;

        await _connection!.InvokeAsync("UnsubscribeFromAirlineFlightAnnouncements", locationId ?? _config.LocationId);
    }

    public async Task DisconnectAsync()
    {
        if (_connection == null)
            return;

        try
        {
            await _connection.StopAsync();
        }
        finally
        {
            await _connection.DisposeAsync();
            _logger.LogInformation("Disconnected from VoxBridge SignalR hub.");
            _connection = null;
        }
    }
}