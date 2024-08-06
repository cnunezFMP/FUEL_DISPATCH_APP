using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator(IBookingServices bookingServices)
        {
            RuleFor(x => x)
                .Must(bookingServices.VehicleHasDriverAssigned);
            RuleFor(x => x)
                .Must(bookingServices.VerifyDisponibility)
                .WithMessage("The vehicle is already reserved for these dates. Or is not ready for proccess. ");
            RuleFor(x => x)
                .NotEmpty()
                .NotNull()
                .Must(bookingServices.CheckDriver)
                .WithMessage("Drive doesn't exist or is unavailable. ");
            RuleFor(x => x)
                .NotEmpty()
                .NotNull()
                .Must(bookingServices.CheckVehicle)
                .WithMessage("Vehicle doesn't exist. ");
        }
    }
}
