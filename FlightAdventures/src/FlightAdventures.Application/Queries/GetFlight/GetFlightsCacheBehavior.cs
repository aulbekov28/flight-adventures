using System.Text.Json;
using FlightAdventures.Domain.Models;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace FlightAdventures.Application.Queries.GetFlight;

public class GetFlightsCacheBehavior : IPipelineBehavior<GetFlightsQuery, ICollection<Flight>>
{
    private readonly ILogger<GetFlightsCacheBehavior> _logger;
    private readonly IDistributedCache  _redis;

    public GetFlightsCacheBehavior(
        ILogger<GetFlightsCacheBehavior> logger,
        IDistributedCache  redis)
    {
        _logger = logger;
        _redis = redis;
    }

    public async Task<ICollection<Flight>> Handle(GetFlightsQuery request, RequestHandlerDelegate<ICollection<Flight>> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetFlightsCacheBehavior");
        
        var cacheKey = $"{request.Origin}-{request.Destination}";
        
        var cachedValue = await _redis.GetStringAsync(cacheKey, token: cancellationToken);
        if (!string.IsNullOrEmpty(cachedValue))
        {
            return JsonSerializer.Deserialize<ICollection<Flight>>(cachedValue);
        }
        
        var response = await next();

        await _redis.SetStringAsync(cacheKey, JsonSerializer.Serialize(response), cancellationToken);
       
        _logger.LogInformation("Handled GetFlightsCacheBehavior");
        return response;
    }
}