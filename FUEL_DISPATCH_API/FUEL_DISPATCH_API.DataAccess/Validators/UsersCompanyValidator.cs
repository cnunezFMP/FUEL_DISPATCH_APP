//using FluentValidation;
//using FUEL_DISPATCH_API.DataAccess.Models;
//using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FUEL_DISPATCH_API.DataAccess.Validators
//{
//    public class UsersCompanyValidator : AbstractValidator<UsersCompanies>
//    {
//        public UsersCompanyValidator(ICompaniesUsersServices companiesUsersServices)
//        {
//            RuleFor(x => x.CompanyId)
//                .NotEmpty()
//                .WithMessage("CompanyId is required");

//            RuleFor(x => x.UserId)
//                .NotEmpty()
//                .WithMessage("UserId is required");

//            RuleFor(x => x)
//                .Must(x => companiesUsersServices.IsUserInCompany(x.UserId, x.CompanyId))
//                .WithMessage("User is already in company. ");
//        }
//    }
//}
