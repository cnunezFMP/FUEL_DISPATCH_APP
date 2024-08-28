using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class DispenserValidator : AbstractValidator<Dispenser>
    {
        public DispenserValidator(IDispenserServices dispenserServices)
        {
            RuleFor(x => x.Code).NotNull().NotEmpty().Must((dispenser, _) =>
            {
                return dispenserServices.DispenserCodeMustBeUnique(dispenser);
            }).WithMessage("Ya existe un dispensador con este codigo. ");
        }
    }
}
