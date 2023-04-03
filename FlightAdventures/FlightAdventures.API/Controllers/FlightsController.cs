using System.Threading.Tasks;
using FlightAdventures.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightAdventures.API.Controllers;

public class FlightsController : ControllerBase
{
    private readonly ISender _mediatr;

    public FlightsController(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(AddFlightCommand command)
    {
        var flight = await _mediatr.Send(command);
        return Ok(flight);
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesDefaultResponseType]
    public async Task<IActionResult> Update(int id, UpdateStatusCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }
        await _mediatr.Send(command);

        return NoContent();
    }
}