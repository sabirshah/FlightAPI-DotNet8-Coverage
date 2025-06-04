using System.Diagnostics.CodeAnalysis;
using FlightInformationAPI.DTOs;
using FluentValidation;

namespace FlightInformationAPI.Validators
{
    [ExcludeFromCodeCoverage]
    public class FlightUpdateDtoValidator : AbstractValidator<FlightUpdateDto>
    {
        public FlightUpdateDtoValidator()
        {
            RuleFor(f => f.FlightNumber)
                .NotEmpty().WithMessage("Flight number is required.")
                .MaximumLength(10);

            RuleFor(f => f.Airline)
                .NotEmpty().WithMessage("Airline is required.");

            RuleFor(f => f.DepartureAirport)
                .NotEmpty().WithMessage("Departure airport is required.")
                .Length(3).WithMessage("Departure airport code must be 3 characters.");

            RuleFor(f => f.ArrivalAirport)
                .NotEmpty().WithMessage("Arrival airport is required.")
                .Length(3).WithMessage("Arrival airport code must be 3 characters.");

            RuleFor(f => f.DepartureTime)
                .NotEmpty().WithMessage("Departure time is required.");

            RuleFor(f => f.ArrivalTime)
                .NotEmpty().WithMessage("Arrival time is required.")
                .GreaterThan(f => f.DepartureTime).WithMessage("Arrival time must be after departure time.");

            RuleFor(f => f.Status.ToLower())
                .NotEmpty().WithMessage("Status is required.")
                .Must(status => new[] { "scheduled", "inair", "delayed", "landed", "cancelled" }
                    .Contains(status))
                .WithMessage("Status must be one of: Scheduled, InAir, Delayed, Landed, Cancelled.");
        }
    }

}
