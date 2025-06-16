namespace SignalRConsoleClient.Models
{
    public class Flight
    {
        public string Id { get; set; } = string.Empty;
        public string Direction { get; set; } = string.Empty;
        public DateTime ScheduledTime { get; set; }
    }
}
