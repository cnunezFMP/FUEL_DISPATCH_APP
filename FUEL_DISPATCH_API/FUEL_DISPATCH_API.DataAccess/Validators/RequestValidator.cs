using Azure.Core;
using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
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
        // DONE: Hacer las validaciones para las entidades en los servicios de Request.
        public RequestValidator(IBookingServices bookingServices,
            IWareHouseMovementServices wareHouseMovementServices,
            IRequestServices requestServices)
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
            RuleFor(x => x.DriverId)
                .Must(bookingServices.CheckDriver)
                .WithMessage("Drive doesn't exist or is unavailable. "); ;

            RuleFor(x => x.VehicleId)
                .Must(wareHouseMovementServices.CheckVehicle)
                .WithMessage("The vehicle is inactive or unavailable. ");

            RuleFor(x => x)
                .Must(requestServices.CheckIfWareHousesHasActiveStatus)
                .WithMessage("WareHouse in not active. ");

        }
    }
}
