using FlightAdventures.Application.Abstractions;
using FlightAdventures.Domain.Enums;
using FlightAdventures.Domain.Models;
using MediatR;

namespace FlightAdventures.Application.Commands.AddFlight;

public class AddFlightCommand : IRequest<Flight>
{
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public FlightStatus Status { get; set; }
}

public class AddFlightHandler : IRequestHandler<AddFlightCommand, Flight>
{
    private readonly IFlightDbContext _context;

    public AddFlightHandler(IFlightDbContext context)
    {
        _context = context;
    }

    public async Task<Flight> Handle(AddFlightCommand request, CancellationToken cancellationToken)
    {
        var flight = new Flight
        {
            Origin = request.Origin,
            Destination = request.Destination,
            Departure = request.Departure,
            Arrival = request.Arrival,
            Status = request.Status,
        };

        _context.Flights.Add(flight);

        await _context.SaveChangesAsync(cancellationToken);

        return flight;
    }
}