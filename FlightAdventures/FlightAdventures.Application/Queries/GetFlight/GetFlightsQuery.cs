using FlightAdventures.Application.Abstractions;
using FlightAdventures.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightAdventures.Application.Queries.GetFlight;

public class GetFlightsQuery : IRequest<ICollection<Flight>>
{
    public string Origin { get; set; }
    public string Destination { get; set; }
}

public class GetFlightsHandler : IRequestHandler<GetFlightsQuery, ICollection<Flight>>
{
    private readonly IFlightDbContext _context;

    public GetFlightsHandler(IFlightDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Flight>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Flights
            .AsNoTracking()
            .Where(x => x.Origin == request.Origin ||
                        (request.Destination != null && x.Destination == request.Destination))
            .ToArrayAsync(cancellationToken);
    }
}