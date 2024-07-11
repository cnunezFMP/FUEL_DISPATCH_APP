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
    public class RoadValidator : AbstractValidator<Road>
    {
        public RoadValidator(IRoadServices roadServices)
        {
            RuleFor(x => x.APoint).NotEmpty().NotNull();
            RuleFor(x => x.BPoint).NotEmpty().NotNull();
            RuleFor(x => x.Code).NotEmpty().NotNull();
            RuleFor(x => x.Code).Must((x, _) =>
            {
                return !roadServices.RoadCodeMustBeUnique(x);
            }).WithMessage("Road with same code already exists. ");
        }
    }
}
