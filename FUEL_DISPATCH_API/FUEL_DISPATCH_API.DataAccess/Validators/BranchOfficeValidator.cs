using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class BranchOfficeValidator : AbstractValidator<BranchOffices>
    {
        public BranchOfficeValidator(IBranchOfficeServices branchOfficeServices)
        {
            RuleFor(x => x.Code)
                .NotEmpty()
                .NotNull()
                .Must((branch, _) => branchOfficeServices.BranchCodeMustBeUnique(branch))
                .WithMessage("Branch office with this code already exist. ");

            RuleFor(x => x.Phone)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.Representative)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.CompanyId)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.FullLocation)
                .NotNull()
                .NotEmpty();
        }

    }
}
