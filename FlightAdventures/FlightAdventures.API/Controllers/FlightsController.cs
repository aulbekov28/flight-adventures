using System.Linq;
using System.Threading.Tasks;
using FlightAdventures.API.Models.FlightDto;
using FlightAdventures.Application.Commands;
using FlightAdventures.Application.Commands.AddFlight;
using FlightAdventures.Application.Commands.UpdateStatus;
using FlightAdventures.Application.Queries;
using FlightAdventures.Application.Queries.GetFlight;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightAdventures.API.Controllers;

public class FlightsController : ControllerBase
{
    private readonly ISender  _mediatr;

    public FlightsController(ISender  mediatr)
    {
        _mediatr = mediatr;
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Get(GetFlightsQuery query)
    {
        var flight = await _mediatr.Send(query);
        return Ok(flight.Select(FlightDto.FromEntity));
    }
    
    [Authorize(Roles = "Moderator")]
    [HttpPost]
    public async Task<IActionResult> Create(AddFlightCommand command)
    {
        var flight = await _mediatr.Send(command);
        return Ok(flight);
    }

    [Authorize(Roles = "Moderator")]
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