using FlightAdventures.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace FlightAdventures.Application.Abstractions;

public interface IFlightDbContext
{
    DbSet<Flight> Flights { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
