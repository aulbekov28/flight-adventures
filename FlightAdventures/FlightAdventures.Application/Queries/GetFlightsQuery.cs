using FlightAdventures.Domain.Models;
using MediatR;

namespace FlightAdventures.Application.Queries;

public class GetFlightsQuery : IRequest<ICollection<Flight>>
{
    public string Origin { get; set; }
    public string Destination { get; set; }
}

public class GetFlightsHandler: IRequestHandler<GetFlightsQuery, ICollection<Flight>>
{
    public GetFlightsHandler()
    {
    }

    public Task<ICollection<Flight>> Handle(GetFlightsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}