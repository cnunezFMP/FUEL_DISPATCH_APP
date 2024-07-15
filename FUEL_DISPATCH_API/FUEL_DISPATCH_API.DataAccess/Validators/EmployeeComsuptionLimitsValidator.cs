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
    public class EmployeeComsuptionLimitsValidator : AbstractValidator<EmployeeConsumptionLimits>
    {
        public EmployeeComsuptionLimitsValidator(IEmployeeComsuptionLimits employeeComsuptionLimitsServices)
        {
            RuleFor(x => x).Must((x, _) =>
            {
                return employeeComsuptionLimitsServices.DriverHasTheMethod(x.Driver!, x.DriverMethodOfComsuption!);
            });
            RuleFor(x => x).Must(x => x.CurrentAmount <= x.LimitAmount).WithMessage("The amount can't be bigger than limit amount. ");
        }
    }
}
