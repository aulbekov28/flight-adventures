using FlightAdventures.Domain.Models;
using MediatR;

namespace FlightAdventures.Application.Commands;

public class UpdateStatusCommand : IRequest<Flight>
{
    public Flight.FlightStatus NewStatus { get; set; }
}