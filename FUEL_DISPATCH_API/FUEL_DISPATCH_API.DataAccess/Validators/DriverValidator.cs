using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class DriverValidator : AbstractValidator<Driver>
    {
        public DriverValidator(IDriversServices driversServices)
        {
            RuleFor(x => x.Identification).NotNull().NotEmpty().MinimumLength(13).MaximumLength(13).Must((identification, _) =>
            {
                return driversServices.CheckIfIdIsUnique(identification);
            }).WithMessage("Ya existe un registro con {PropertyName}. Se ingreso {PropertyValue}. ");
            RuleFor(x => x.Email).EmailAddress().Must((driverEmail, _) =>
            {
                return driversServices.IsEmailUnique(driverEmail);
            }).WithMessage("El {PropertyName} ya esta registrado. Se ingreso {PropertyValue}. ");

            RuleFor(x => x.VehicleId).Must((driverVehicleId, _) =>
            {
                return driversServices.VehicleIdHasValue(driverVehicleId);
            }).WithMessage("No existe un vehiculo con este Id. Esto ocurrio en {PropertyName}. ");
        }
    }
}
