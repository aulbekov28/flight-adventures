using FlightAdventures.Domain.Enums;
using MediatR;

namespace FlightAdventures.Application.Commands;

public class UpdateStatusCommand : IRequest
{
    public int Id { get; set; }
    
    public FlightStatus NewStatus { get; set; }
}

public class UpdateStatusHandler: IRequestHandler<UpdateStatusCommand>
{
    public UpdateStatusHandler()
    {
    }
    
    public Task Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}