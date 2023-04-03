using FluentValidation;

namespace FlightAdventures.Application.Queries;

public class GetFlightsQueryValidator : AbstractValidator<GetFlightsQuery>
{
    public GetFlightsQueryValidator()
    {
        RuleFor(v => v.Origin)
            .NotEmpty().WithMessage("Origin is required.")
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");
        
        RuleFor(v => v.Destination)
            .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");
    }
}