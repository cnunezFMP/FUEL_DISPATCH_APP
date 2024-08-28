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
                .WithMessage("El vehiculo ya esta reservado para estas fechas, o no esta listo para procesar. ");

            RuleFor(x => x)
                .NotEmpty()
                .NotNull()
                .Must(bookingServices.CheckDriver)
                .WithMessage("El conductor no existe o no es valido. ");

            RuleFor(x => x)
                .NotEmpty()
                .NotNull()
                .Must(bookingServices.CheckVehicle)
                .WithMessage("El vehiculo no existe. ");
        }
    }
}
