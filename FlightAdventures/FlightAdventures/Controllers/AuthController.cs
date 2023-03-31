﻿using System.Threading;
using System.Threading.Tasks;
using FlightAdventures.Requests.Auth;
using FlightAdventures.Responses;
using Microsoft.AspNetCore.Mvc;

namespace FlightAdventures.Controllers;

[Controller]
[Route("auth")]
public class AuthController : ControllerBase
{
    public AuthController()
    {
        
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> MatchActors(AuthReqeust request, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        var token = string.Empty;
        return new ActionResult<AuthResponse>(
            new AuthResponse(token));
    }
}