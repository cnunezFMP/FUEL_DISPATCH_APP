using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class BranchIslandValidator : AbstractValidator<BranchIsland>
    {
        public BranchIslandValidator(IBranchIslandServices branchIslandServices)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull()
                .Must((island, _) =>
            {
                return !branchIslandServices.BranchIslandCodeMustBeUnique(island);
            }).WithMessage("A branch island with this code already exist. ");
        }
    }
}
