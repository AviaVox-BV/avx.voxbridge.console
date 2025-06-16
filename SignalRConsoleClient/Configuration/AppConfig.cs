namespace SignalRConsoleClient.Configuration
{
    public class AppConfig
    {
        public string ClientId { get; set; } = string.Empty;
        public string TenantId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;
        public string LocationId { get; set; } = string.Empty;
        public Dictionary<string, string> Environments { get; set; } = new();
    }
}
