using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class UsersBranchOfficeValidator : AbstractValidator<UsersBranchOffices>
    {
        public UsersBranchOfficeValidator(IUsersBranchOfficesServices usersBranchOfficesServices)
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("UserId is required");
            RuleFor(x => x.BranchOfficeId)
                .NotEmpty()
                .WithMessage("BranchOfficeId is required");
            RuleFor(x => x)
                .Must(x => usersBranchOfficesServices.IsUserInBranchOffice(x.UserId, x.BranchOfficeId));

        }
    }
}
