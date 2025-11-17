using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SignalRConsoleClient.Configuration;
using SignalRConsoleClient.Services;
using SignalRConsoleClient.Utils;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((context, config) =>
    {
        var env = context.HostingEnvironment;

        config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
              .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        services.Configure<AppConfig>(context.Configuration);
        services.AddSingleton<AuthService>();
        services.AddSingleton<SignalRService>();
        services.AddSingleton<HealthCheckService>();
        services.AddSingleton<CommandLineOptions>();
        services.AddLogging(builder => builder.AddConsole());
    })
    .Build();

var options = host.Services.GetRequiredService<CommandLineOptions>();
options.Parse(args);

var cfg = host.Services.GetRequiredService<IOptions<AppConfig>>().Value;
bool hasLocal = cfg.Environments.ContainsKey("local");

if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("ENV")))
{
    while (true)
    {
        Console.WriteLine("Choose environment:");
        if (hasLocal)
            Console.WriteLine("1 = Local");
        Console.WriteLine($"{(hasLocal ? 2 : 1)} = Test");
        Console.WriteLine($"{(hasLocal ? 3 : 2)} = Production");
        Console.WriteLine("0 = Exit");
        var envSelection = Console.ReadLine()?.Trim();

        string? env = null;
        if (hasLocal)
        {
            env = envSelection switch
            {
                "1" => "local",
                "2" => "test",
                "3" => "prod",
                "0" => null,
                _ => null
            };
        }
        else
        {
            env = envSelection switch
            {
                "1" => "test",
                "2" => "prod",
                "0" => null,
                _ => null
            };
        }

        if (env == null)
        {
            if (envSelection == "0")
            {
                Console.WriteLine("Exiting...");
                return;
            }

            Console.WriteLine("Invalid environment selection. Try again.");
            continue;
        }

        Environment.SetEnvironmentVariable("ENV", env);
        break;
    }
}

var logger = host.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Main");
var authService = host.Services.GetRequiredService<AuthService>();
var signalRService = host.Services.GetRequiredService<SignalRService>();
var healthCheckService = host.Services.GetRequiredService<HealthCheckService>();

bool keepRunning = true;
while (keepRunning)
{
    Console.WriteLine("Choose action:");
    Console.WriteLine("1 = Subscribe to Flights");
    Console.WriteLine("2 = Subscribe to Announcements");
    Console.WriteLine("3 = Health Check");
    Console.WriteLine("0 = Exit");
    var action = Console.ReadLine();

    try
    {
        string fullToken = await authService.GetTokenAsync();

        switch (action)
        {
            case "0":
                Console.WriteLine("Exiting...");
                keepRunning = false;
                break;

            case "1":
                if (string.IsNullOrWhiteSpace(options.LocationId))
                {
                    Console.WriteLine("Enter LocationId (or press Enter to use default):");
                    var locId = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(locId))
                        options.LocationId = locId;
                }

                await signalRService.ConnectAsync(fullToken);
                await signalRService.SubscribeFlightsAsync(options.LocationId);

                Console.WriteLine("Listening for flight updates. Press any key to stop and return to menu...");
                Console.ReadKey();

                await signalRService.UnsubscribeFlightsAsync(options.LocationId);

                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------------------------");
                break;
            case "2":
                if (string.IsNullOrWhiteSpace(options.LocationId))
                {
                    Console.WriteLine("Enter LocationId (or press Enter to use default):");
                    var locId = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(locId))
                        options.LocationId = locId;
                }

                await signalRService.ConnectAsync(fullToken);
                await signalRService.SubscribeFlightAnnouncementsAsync(options.LocationId);

                Console.WriteLine("Listening for announcements. Press any key to stop and return to menu...");
                Console.ReadKey();

                await signalRService.UnsubscribeFlightAnnouncementsAsync(options.LocationId);

                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------------------------");
                break;

            case "3":
                Console.WriteLine("Enter message to echo");
                var msg = Console.ReadLine();

                var healthy = await healthCheckService.CheckConnectionAsync();
                Console.WriteLine($"Health Check: {(healthy ? "OK" : "Failed")}");
                Console.WriteLine();

                if (!healthy)
                    break;

                var token = await authService.GetTokenAsync();
                await signalRService.ConnectAsync(token);

                try
                {
                    await signalRService.EchoAsync(msg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Echo failed: {ex.Message}");
                }

                Console.WriteLine("");
                Console.WriteLine("-----------------------------------------------------------------");
                break;

            default:
                logger.LogWarning("Unknown action selected.");
                break;
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex.Message, "An error occurred while processing the action.");
    }
}
