using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class BranchOfficeValidator : AbstractValidator<BranchOffices>
    {
        public BranchOfficeValidator(IBranchOfficeServices branchOfficeServices)
        {
            RuleFor(x => x.Code).NotEmpty().NotNull().Must((branch, _) =>
            {
                return !branchOfficeServices.BranchCodeMustBeUnique(branch);    
            }).WithMessage("Branch Office code can't be null. ");
            RuleFor(x => x.Phone).NotNull().NotEmpty();
            RuleFor(x => x.Representative).NotNull().NotEmpty();
            RuleFor(x => x.CompanyId).NotNull().NotEmpty();
            RuleFor(x => x.FullLocation).NotNull().NotEmpty();
            
        }

    }
}
