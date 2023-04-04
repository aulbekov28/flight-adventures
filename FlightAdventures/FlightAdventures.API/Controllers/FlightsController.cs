using System.Linq;
using System.Threading.Tasks;
using FlightAdventures.API.Models.FlightDto;
using FlightAdventures.Application.Commands.UpdateStatus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FlightAdventures.API.Controllers;

public class FlightsController : BaseApiController
{
    private readonly ISender  _mediatr;

    public FlightsController(ISender  mediatr)
    {
        _mediatr = mediatr;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetFlightsQuery query)
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