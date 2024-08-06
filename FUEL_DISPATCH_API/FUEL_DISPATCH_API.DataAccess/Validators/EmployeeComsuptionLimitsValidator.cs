using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class EmployeeComsuptionLimitsValidator : AbstractValidator<EmployeeConsumptionLimits>
    {
        public EmployeeComsuptionLimitsValidator(IEmployeeComsuptionLimitsServices employeeComsuptionLimitsServices)
        {
            RuleSet("InPost", () =>
            {
                RuleFor(x => x.DriverId & (int)x.DriverMethodOfComsuptionId).Must((x, _) =>
                {
                    return employeeComsuptionLimitsServices.DriverHasTheMethod
                    (
                        x.DriverId!,
                        (int)x.DriverMethodOfComsuptionId!
                    );
                }).WithMessage("This driver has this method assigned. ");
            });

            RuleFor(x => x)
                .Must(x => x.CurrentAmount <= x.LimitAmount)
                .WithMessage("The amount can't be bigger than limit amount. ");
        }
    }
}
