using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class DispatchValidator : AbstractValidator<WareHouseMovement>
    {
        public DispatchValidator(IWareHouseMovementServices wareHouseMovementServices)
        {
            RuleFor(x => x.Vehicle).Must((vehicle, _) =>
            {
                return !wareHouseMovementServices.CheckVehicle(vehicle);
            }).WithMessage("{PropertyName} is inactive or unavailable. ").When(x => x.VehicleId.HasValue);
            RuleFor(x => x.Qty).Must((qty, _) =>
            {
                return wareHouseMovementServices.QtyCantBeZero(qty);
            }).WithMessage("{PropertyName} no puede ser cero. ");
            RuleFor(x => x.Qty).Must((movement, _) =>
            {
                return wareHouseMovementServices.CheckWareHouseStock(movement);
            }).WithMessage("No stock in warehouse, or stock qty is lesser than specified qty. ");
            RuleFor(x => x.Driver).Must((driver, _) =>
            {
                return !wareHouseMovementServices.CheckDriver(driver);
            }).WithMessage("{PropertyName} is inactive or unavailable.");
            RuleFor(x => x.BranchOffice).Must((branch, _) =>
            {
                return !wareHouseMovementServices.CheckBranchOffice(branch);
            }).WithMessage("{PropertyName} is inactive or unavailable. ");
            RuleFor(x => x.Dispenser).Must((branch, _) =>
            {
                return !wareHouseMovementServices.CheckBranchOffice(branch);
            }).WithMessage("PropertyName} is inactive or unavailable. ");

            RuleFor(x => x).Must(warehouseMovement => wareHouseMovementServices.CheckIfProductIsInTheWareHouse(warehouseMovement)).WithMessage("The product isn't in the specified warehouse. ");
            RuleFor(x => x.WareHouse).Must((wareHouse, _) =>
            {
                return wareHouseMovementServices.CheckIfWareHousesHasActiveStatus(wareHouse);
            }).WithMessage("WareHouse in not active. ");
            RuleFor(x => x.ToWareHouse).Must((wareHouse, _) =>
            {
                return wareHouseMovementServices.CheckIfWareHousesHasActiveStatus(wareHouse);
            }).When(x => x.Type is "Transferencia").WithMessage("One or both of the warehose are not active. ");
            RuleFor(x => x.WareHouse).Must((wareHouseMovement, _) =>
            {
                return !wareHouseMovementServices.WillStockFallBelowMinimum(wareHouseMovement);
            }).WithMessage("The stock level will fall below the minimum quantity allowed. ");
        }
    }
}
