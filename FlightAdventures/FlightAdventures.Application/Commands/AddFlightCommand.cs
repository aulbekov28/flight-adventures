using FlightAdventures.Domain.Enums;
using FlightAdventures.Domain.Models;
using MediatR;

namespace FlightAdventures.Application.Commands;

public class AddFlightCommand : IRequest<Flight>
{
    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public FlightStatus Status { get; set; }
}