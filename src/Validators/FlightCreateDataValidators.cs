using System.Diagnostics.CodeAnalysis;
using FlightInformationAPI.DTOs;
using FluentValidation;

namespace FlightInformationAPI.Validators
{
    [ExcludeFromCodeCoverage]
    public class FlightCreateDtoValidator : AbstractValidator<FlightCreateDto>
    {
        public FlightCreateDtoValidator()
        {
            RuleFor(x => x.FlightNumber).NotEmpty();
            RuleFor(x => x.Airline).NotEmpty();
            RuleFor(x => x.DepartureTime).LessThan(x => x.ArrivalTime);
        }
    }
}
