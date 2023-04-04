using System.Threading;
using System.Threading.Tasks;
using FlightAdventures.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FlightAdventures.API.Controllers;

[Controller]
[Route("auth")]
public class AuthController : ControllerBase
{
    public AuthController()
    {
        
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> MatchActors(CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        var token = string.Empty;
        return new ActionResult<AuthResponse>(
            new AuthResponse(token));
    }
}