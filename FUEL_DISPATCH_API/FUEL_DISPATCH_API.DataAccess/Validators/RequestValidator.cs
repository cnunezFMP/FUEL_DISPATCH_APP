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
                RuleFor(x => x.ToWareHouseId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type is MovementsTypesEnum.Transferencia);
            });

            RuleFor(x => x)
                .Must(requestServices.CheckDriver)
                .WithMessage("Drive doesn't exist or is unavailable. "); ;

            // DONE: Hacer nativo el servicio. 
            RuleFor(x => x)
                .Must(x => requestServices.CheckVehicle(x))
                .WithMessage("The vehicle is inactive or unavailable. ");

            RuleFor(x => x)
                .Must(requestServices.CheckIfWareHousesHasActiveStatus)
                .WithMessage("WareHouse in not active. ");

            RuleFor(x => x.Qty).Must(x => x > ValidationConstants.ZeroGallons);

            RuleFor(x => x)
                .Must(requestServices.CheckVehicle)
                .WithMessage("The vehicle may be inactive or unavailable. ");
        }
    }
}
