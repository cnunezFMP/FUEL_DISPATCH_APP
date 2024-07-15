using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
namespace FUEL_DISPATCH_API.DataAccess.Validators
{
    public class WareHouseMovementValidator : AbstractValidator<WareHouseMovement>
    {
        public WareHouseMovementValidator(IWareHouseMovementServices wareHouseMovementServices)
        {
            RuleFor(x => x.Vehicle).Must(x =>
            {
                return wareHouseMovementServices.CheckVehicle(x!.Id);
            }
            ).WithMessage("{PropertyName} is inactive or unavailable. ").When(x => x.VehicleId.HasValue);
            RuleFor(x => x.Qty).Must((qty, _) =>
            {
                return wareHouseMovementServices.QtyCantBeZero(qty);
            }).WithMessage("{PropertyName} no puede ser cero. ");
            RuleFor(x => x.Qty).Must((movement, _) =>
            {
                return wareHouseMovementServices.CheckWareHouseStock(movement);
            }).WithMessage("No stock in warehouse, or stock qty is lesser than specified qty. ");
            RuleFor(x => x.Driver).Must(x => wareHouseMovementServices.CheckDriver(x!.Id)).WithMessage("{PropertyName} is inactive or unavailable.");
            RuleFor(x => x.BranchOffice).Must((branch, _) =>
            {
                return !wareHouseMovementServices.CheckBranchOffice(branch);
            }).WithMessage("{PropertyName} is inactive or unavailable. ");
            RuleFor(x => x.Dispenser).Must((branch, _) =>
            {
                return !wareHouseMovementServices.CheckBranchOffice(branch);
            }).WithMessage("PropertyName} is inactive or unavailable. ");
            RuleFor(x => x).Must(x => wareHouseMovementServices.CheckIfProductIsInTheWareHouse(x)).WithMessage("The product isn't in the specified warehouse. ");
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
            }).WithMessage("The stock level will fall below the minimum quantity allowed. ").When(x => x.Type is "Salida" || x.Type is "Transferencia");
            RuleFor(x => x.WareHouse).Must((wareHouseMovement, _) =>
            {
                return wareHouseMovementServices.WillStockFallMaximun(wareHouseMovement);
            }).When(x => x.Type is "Entrada" || x.Type is "Transferencia").WithMessage("The input quantity will exceed the maximum warehouse quantity. ");
            // TODO: Test this validation.
            RuleFor(x => x)
                .Must(x => wareHouseMovementServices.SetRequestForMovement(x))
                .When(x => x.RequestId.HasValue
                && x.Request!.Status is not ValidationConstants.PendingStatus
                && x.Request!.Status is not ValidationConstants.RejectedStatus
                && x.Request!.Status is not ValidationConstants.CanceledStatus)
                .WithMessage(x => $"Unfortunately, this request is not ready to be processed/used at this time. Request status is {x.Request!.Status}");
        }
    }
}
