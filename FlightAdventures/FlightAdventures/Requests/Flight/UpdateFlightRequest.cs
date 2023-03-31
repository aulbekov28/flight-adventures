namespace FlightAdventures.Requests.Flight;

public record StatusUpdateRequest
{
    public int NewStatus { get; set; }
}