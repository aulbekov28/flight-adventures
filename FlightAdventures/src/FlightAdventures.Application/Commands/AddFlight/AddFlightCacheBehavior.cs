using System.Text.Json;
using FlightAdventures.Domain.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FlightAdventures.Application.Commands.AddFlight;

public class AddFlightCacheBehavior : IPipelineBehavior<AddFlightCommand, Flight>
{
    private readonly ILogger<AddFlightCacheBehavior> _logger;
    private readonly IDistributedCache _cahche;

    public AddFlightCacheBehavior(
        ILogger<AddFlightCacheBehavior> logger,
        IDistributedCache cahche)
    {
        _logger = logger;
        _cahche = cahche;
    }

    public async Task<Flight> Handle(AddFlightCommand request, RequestHandlerDelegate<Flight> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling AddFlightCacheBehavior");

        var newFlight = await next();
        
        var cacheKey = $"{request.Origin}-{request.Destination}";
        
        var cachedValue = await _cahche.GetStringAsync(cacheKey, cancellationToken);
        if (!string.IsNullOrEmpty(cachedValue))
        {
            // TODO probably incorrect - reconsider data model in Redis
            var flights = JsonSerializer.Deserialize<ICollection<Flight>>(cachedValue);
            flights.Add(newFlight);
            await _cahche.SetStringAsync(cacheKey, JsonSerializer.Serialize(flights), cancellationToken);
        }
        
        _logger.LogInformation("Handled GetFlightsCacheBehavior");
        return newFlight;
    }
}