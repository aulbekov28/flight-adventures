using FlightAdventures.Application.Abstractions;
using FlightAdventures.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FlightAdventures.Application.Commands;

public class UpdateStatusCommand : IRequest
{
    public int Id { get; set; }
    
    public FlightStatus NewStatus { get; set; }
}

public class UpdateStatusHandler: IRequestHandler<UpdateStatusCommand>
{
    private readonly IFlightDbContext _context;

    public UpdateStatusHandler(IFlightDbContext context)
    {
        _context = context;
    }
    
    public async Task Handle(UpdateStatusCommand request, CancellationToken cancellationToken)
    {
        var flight = await _context
            .Flights
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken);
        
        if (flight == null)
        {
            throw new Exception($"Flight not found {request.Id}"); // TODO crete a proper exceptions
        }
                    
        flight.Status = request.NewStatus;

        await _context.SaveChangesAsync(cancellationToken);
    }
}