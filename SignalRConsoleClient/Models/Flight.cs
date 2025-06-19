namespace SignalRConsoleClient.Models
{
    public class Flight
    {
        public string? Id { get; set; }
        public required string Direction { get; set; }
        public string? Terminal { get; set; }
        public bool Schengen { get; set; }
        public DateTime? UtcLastUpdated { get; set; }
        public required DateTime ScheduledTime { get; set; }
        public DateTime? EstimatedTime { get; set; }
        public DateTime? ActualTime { get; set; }

        public required Status Status { get; set; }
        public KindOfFlight? KindOfFlight { get; set; }
        public ServiceType? ServiceType { get; set; }
        public Aircraft? Aircraft { get; set; }
        public ReasonOfDelay? ReasonOfDelay { get; set; }
        public Diversion? Diversion { get; set; }

        public required List<AirlineFlightNumber> AirlineFlightNumbers { get; set; } = [];
        public required List<RoutePoint>? Route { get; set; }
        public List<string>? Gates { get; set; }
        public List<string>? PreviousGates { get; set; }
        public List<string>? LuggageLocationCodes { get; set; }
    }
}
