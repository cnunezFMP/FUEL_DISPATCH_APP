using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class BookingValidator : AbstractValidator<Booking>
    {
        public BookingValidator(IBookingServices bookingServices)
        {
            RuleFor(x => x).Must(bookingServices.VehicleHasDriverAssigned);
            RuleFor(x => x).Must(bookingServices.VerifyDisponibility).WithMessage("The vehicle is already reserved for these dates. ");
            //RuleFor(x => x).NotEmpty().NotNull().Must(bookingServices.CheckDriver);
            RuleFor(x => x).NotEmpty().NotNull().Must(bookingServices.CheckVehicle);
        }
    }
}
