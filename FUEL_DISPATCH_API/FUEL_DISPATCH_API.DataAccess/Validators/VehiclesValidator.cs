using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class VehiclesValidator : AbstractValidator<Vehicle>
    {
        public VehiclesValidator(IVehiclesServices vehiclesServices)
        {
            RuleFor(x => x.Plate)
                .MinimumLength(6)
                .MaximumLength(10)
                .WithMessage("Ingrese una placa valida. ");

            RuleFor(x => x.VIN)
                .MinimumLength(17)
                .MaximumLength(17)
                .NotNull()
                .NotEmpty()
                .WithMessage("Ingrese un numero de chasis valido. ");

            RuleFor(x => x.Ficha)
                .NotNull()
                .NotNull()
                .Must((vehicleToken, _) =>
                {
                    return vehiclesServices.FichaMustBeUnique(vehicleToken);
                }).WithMessage("Ya existe un vehiculo con esta ficha. ");
        }
    }
}
