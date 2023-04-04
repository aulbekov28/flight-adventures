using System.Text.Json;
using FlightAdventures.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace FlightAdventures.Application.Commands.AddFlight;

public class AddFlightCacheBehavior : IPipelineBehavior<AddFlightCommand, Flight>
{
    private readonly ILogger<AddFlightCacheBehavior> _logger;
    private readonly IConnectionMultiplexer _redis;

    public AddFlightCacheBehavior(
        ILogger<AddFlightCacheBehavior> logger,
        IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public async Task<Flight> Handle(AddFlightCommand request, RequestHandlerDelegate<Flight> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling AddFlightCacheBehavior");

        var newFlight = await next();
        
        var cacheKey = $"{request.Origin}-{request.Destination}";
        
        var database = _redis.GetDatabase();
        var cachedValue = await database.StringGetAsync(cacheKey);
        if (!cachedValue.IsNull)
        {
            // TODO probably incorrect - reconsider data model in Redis
            var flights = JsonSerializer.Deserialize<ICollection<Flight>>(cachedValue);
            flights.Add(newFlight);
            await database.StringSetAsync(cacheKey, JsonSerializer.Serialize(flights), TimeSpan.FromMinutes(10));
        }
        
        _logger.LogInformation("Handled GetFlightsCacheBehavior");
        return newFlight;
    }
}