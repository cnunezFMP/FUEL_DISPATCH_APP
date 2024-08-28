using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using System;
using System.Linq;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class CompanyValidator : AbstractValidator<Companies>
    {
        public CompanyValidator(ICompaniesServices companiesServices)
        {
            
            RuleFor(x => x.CompanyRNC)
                .NotEmpty()
                .NotNull()
                .MaximumLength(9)
                .MinimumLength(9)
                .Must((company, _) => companiesServices.IsCompanyUnique(company));

            RuleFor(x => x.EmailAddress)
                .EmailAddress();

            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty();
        }
    }
}
