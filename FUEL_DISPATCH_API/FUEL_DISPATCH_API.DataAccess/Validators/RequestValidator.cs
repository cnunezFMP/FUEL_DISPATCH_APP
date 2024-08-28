using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class RequestValidator : AbstractValidator<WareHouseMovementRequest>
    {
        // DONE: Hacer las validaciones para las entidades en los servicios de Request.
        public RequestValidator(IRequestServices requestServices)
        {
            RuleSet("WareHouses", () =>
            {
                RuleFor(x => x.WareHouseId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0);
            });

            RuleFor(x => x)
                .Must(requestServices.CheckDriver)
                .WithMessage("El conductor no existe o es invalido. "); ;

            // DONE: Hacer nativo el servicio. 
            RuleFor(x => x)
                .Must(x => requestServices.CheckVehicle(x))
                .WithMessage("El vehiculo esta inactivo o no es valido. ");

            RuleFor(x => x)
                .Must(requestServices.CheckIfWareHousesHasActiveStatus)
                .WithMessage("El almacen esta inactivo. ");

            RuleFor(x => x.Qty).Must(x => x > ValidationConstants.ZeroGallons);

            RuleFor(x => x)
                .Must(requestServices.CheckVehicle)
                .WithMessage("El vehiculo esta inactivo o no es valido. ");
        }
    }
}
