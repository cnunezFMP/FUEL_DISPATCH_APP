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
    public class ZoneValidator : AbstractValidator<Zone>
    {
        public ZoneValidator(IZoneServices zoneServices)
        {
            // DONE: Terminar el validador. 
            RuleFor(x => x.FullZoneSpecs).NotNull().NotEmpty();
            RuleFor(x => x.Code).Must((x, _)
                => zoneServices.ZoneCodeMustBeUnique(x))
                .WithMessage("A zone with the same code exists. Inserted value is {PropertyValue}. ");
        }
    }
}
