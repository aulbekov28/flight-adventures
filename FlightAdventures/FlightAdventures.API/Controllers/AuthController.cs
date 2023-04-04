using System.Threading;
using System.Threading.Tasks;
using FlightAdventures.API.Requests;
using FlightAdventures.API.Responses;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FlightAdventures.API.Controllers;

public class AuthController : BaseApiController
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AuthController(SignInManager<ApplicationUser> signInManager)
    {
        _signInManager = signInManager;
    }
    
    [HttpPost]
    public async Task<ActionResult<AuthResponse>> SignIn(AuthRequest authRequest, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(authRequest.User, authRequest.Password, true, lockoutOnFailure: false);
        return new ActionResult<AuthResponse>(
            new AuthResponse(result.Succeeded));
    }
}