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
        public EmployeeComsuptionLimitsValidator(IEmployeeComsuptionLimitsServices employeeComsuptionLimitsServices)
        {
            RuleFor(x => x.DriverId & x.DriverMethodOfComsuptionId).Must((x, _) =>
            {
                return employeeComsuptionLimitsServices.DriverHasTheMethod(x.DriverId!, x.DriverMethodOfComsuptionId!);
            }).WithMessage("This driver has this method. ");
            RuleFor(x => x).Must(x => x.CurrentAmount <= x.LimitAmount).WithMessage("The amount can't be bigger than limit amount. ");

           
        }
    }
}
