namespace SignalRConsoleClient.Models
{
    public class Announcement
    {
        public string? Id { get; set; }
        public string? FlightId { get; set; }
        public string? AnnouncementText { get; set; }
        public string? AudioFileId { get; set; }
        public bool? IsFlightRelated { get; set; }
        public List<string>? Zones { get; set; }
        public DateTime? GenerationTimeUTC { get; set; }
        public DateTime? GenerationTimeLocal { get; set; }
        public DateTime? LastUpdatedUTC { get; set; }
    }
}
