using System;
using FlightAdventures.Domain.Enums;
using FlightAdventures.Domain.Models;

namespace FlightAdventures.API.Models.FlightDto;

internal class FlightDto
{
    public int Id { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTimeOffset Departure { get; set; }
    public DateTimeOffset Arrival { get; set; }
    public FlightStatus Status { get; set; }
    
    public static FlightDto FromEntity(Flight flight)
    {
        return new FlightDto
        {
            Id = flight.Id,
            Origin = flight.Origin,
            Destination = flight.Destination,
            Arrival = flight.Arrival,
            Departure = flight.Departure,
            Status = flight.Status, 
        };
    }
}