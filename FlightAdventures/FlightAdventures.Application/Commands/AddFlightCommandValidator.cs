using FluentValidation;

namespace FlightAdventures.Application.Commands;

public class AddFlightCommandValidator : AbstractValidator<AddFlightCommand>
{
    public AddFlightCommandValidator()
    {
        RuleFor(v => v.Origin)
            .NotEmpty().WithMessage("Origin is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters");
        
        RuleFor(v => v.Destination)
            .NotEmpty().WithMessage("Destination is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters");

        RuleFor(v => v.Departure)
            .LessThan(v => v.Arrival)
            .WithMessage("Arrival time must be greater than Departure");
    }
}