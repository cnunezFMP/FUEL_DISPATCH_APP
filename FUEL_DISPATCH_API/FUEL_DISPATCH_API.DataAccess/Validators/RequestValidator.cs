using Azure.Core;
using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class RequestValidator : AbstractValidator<WareHouseMovementRequest>
    {
        // TODO: Hacer las validaciones para las entidades en los servicios de Request.
        public RequestValidator() 
        {
            RuleSet("WareHouses", () =>
            {
                RuleFor(x => x.WareHouseId).NotEmpty().NotNull().NotEqual(0);
                RuleFor(x => x.ToWareHouseId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type is MovementsTypesEnum.Transferencia);
            });
            //RuleFor(x => x.);
        }   
    }
}
