namespace SignalRConsoleClient.Models
{
    public class Announcement
    {
        public string? Id { get; set; }
        public string? FlightId { get; set; }
        public List<AnnouncementTextObj>? AnnouncementTexts { get; set; }
        public string? Description { get; set; }
        public List<AudioFile>? AudioFiles { get; set; }
        public bool? IsFlightRelated { get; set; }
        public bool? IsNewOrUpdated { get; set; }
        public List<string>? Zones { get; set; }
        public DateTime? GenerationTimeUTC { get; set; }
        public DateTime? GenerationTimeLocal { get; set; }
        public DateTime? LastUpdatedUTC { get; set; }
    }
}
