namespace SignalRConsoleClient.Utils;
public class CommandLineOptions
{
    public bool HealthCheck { get; set; }
    public string? Endpoint { get; set; }
    public string? LocationId { get; set; }

    public void Parse(string[] args)
    {
        foreach (var arg in args)
        {
            if (arg.StartsWith("--health")) HealthCheck = true;
            else if (arg.StartsWith("--endpoint=")) Endpoint = arg.Split('=')[1];
            else if (arg.StartsWith("--locationid=")) LocationId = arg.Split('=')[1];
        }
    }
}
