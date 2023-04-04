using System.Text.Json;
using FlightAdventures.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace FlightAdventures.Application.Queries.GetFlight;

public class GetFlightsCacheBehavior : IPipelineBehavior<GetFlightsQuery, ICollection<Flight>>
{
    private readonly ILogger<GetFlightsCacheBehavior> _logger;
    private readonly IConnectionMultiplexer _redis;

    public GetFlightsCacheBehavior(
        ILogger<GetFlightsCacheBehavior> logger,
        IConnectionMultiplexer redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public async Task<ICollection<Flight>> Handle(GetFlightsQuery request, RequestHandlerDelegate<ICollection<Flight>> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetFlightsCacheBehavior");
        
        var cacheKey = $"{request.Origin}-{request.Destination}";
        
        var database = _redis.GetDatabase();
        var cachedValue = await database.StringGetAsync(cacheKey);
        if (!cachedValue.IsNull)
        {
            return JsonSerializer.Deserialize<ICollection<Flight>>(cachedValue);
        }
        
        var response = await next();

        await database.StringSetAsync(cacheKey, JsonSerializer.Serialize(response), TimeSpan.FromMinutes(10));
       
        _logger.LogInformation("Handled GetFlightsCacheBehavior");
        return response;
    }
}