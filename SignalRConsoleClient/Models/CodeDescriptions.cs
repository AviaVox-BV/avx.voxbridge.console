namespace SignalRConsoleClient.Models
{
    public class AirlineFlightNumber
    {
        public required string AirlineId { get; set; }
        public string? AirlineCode { get; set; }
        public string? AirlineDescription { get; set; }
        public string? FlightNumber { get; set; }
        public bool Main { get; set; }
    }
    public class Status : CodeDescriptionBase { }
    public class KindOfFlight : CodeDescriptionBase { }
    public class ServiceType : CodeDescriptionBase { }
    public class Aircraft : CodeDescriptionBase { }
    public class ReasonOfDelay : CodeDescriptionBase { }
    public class Diversion : CodeDescriptionBase { }
    public class RoutePoint : CodeDescriptionBase { }
}
