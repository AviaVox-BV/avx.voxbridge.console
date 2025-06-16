namespace SignalRConsoleClient.Models
{
    public class Announcement
    {
        public string Id { get; set; } = string.Empty;
        public string AnnouncementText { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string FlightNumber { get; set; } = string.Empty;
        public DateTime GenerationTimeUTC { get; set; }
    }
}
