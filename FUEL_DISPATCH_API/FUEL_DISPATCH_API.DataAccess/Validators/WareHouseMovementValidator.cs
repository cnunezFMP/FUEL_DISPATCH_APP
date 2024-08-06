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
           /* RuleFor(x => x.RoadId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Salida);*/
            RuleFor(x => x.DispenserId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Salida);

            RuleFor(x => x.Odometer)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Salida);
            RuleFor(x => x.ToWareHouseId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Transferencia);
            RuleFor(x => x.DriverId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Salida);

           

            RuleFor(x => x.VehicleId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Salida);

            RuleFor(x => x.FuelMethodOfComsuptionId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .When(x => x.Type == MovementsTypesEnum.Salida && x.DriverId.HasValue);
            RuleFor(x => x).Must(x =>
            {
                // DONE: Corregir(CheckVehicle). Pasar el objeto, y no el VehicleId.
                return wareHouseMovementServices.CheckVehicle(x);
            }).WithMessage("{PropertyName} is inactive or unavailable. ")
              .When(x => x.VehicleId.HasValue);
            RuleFor(x => x.Qty).Must((qty, _) =>
            {
                return wareHouseMovementServices.QtyCantBeZero(qty);
            }).WithMessage("{PropertyName} no puede ser cero. ");
            RuleFor(x => x.Qty).Must((movement, _) =>
            {
                return wareHouseMovementServices.CheckWareHouseStock(movement);
            }).When(x => x.Type is MovementsTypesEnum.Salida ||
               x.Type is MovementsTypesEnum.Transferencia)
              .WithMessage("No stock in warehouse, or stock qty is lesser than specified qty. ");

            RuleFor(x => x)
                .Must(wareHouseMovementServices.CheckDriver)
                .WithMessage("Driver is inactive or unavailable.");

            RuleFor(x => x.BranchOffice).Must((branch, _) =>
            {
                return !wareHouseMovementServices.CheckBranchOffice(branch);
            }).WithMessage("{PropertyName} is inactive or unavailable. ");
            RuleFor(x => x.Dispenser).Must((dispenser, _) =>
            {
                return wareHouseMovementServices.CheckDispenser(dispenser);
            }).WithMessage("{PropertyName} is inactive or unavailable. ");
            RuleFor(x => x)
                .Must(wareHouseMovementServices.CheckIfProductIsInTheWareHouse)
                .When(x => x.Type is MovementsTypesEnum.Salida || x.Type is MovementsTypesEnum.Transferencia)
                .WithMessage("The product isn't in the specified warehouse. ");
            RuleFor(x => x).Must((wareHouse, _) =>
            {
                return wareHouseMovementServices.CheckIfWareHousesHasActiveStatus(wareHouse);
            }).WithMessage("WareHouse in not active. ");
            RuleFor(x => x.ToWareHouse)
                .Must((wareHouse, _) =>
            {
                return wareHouseMovementServices.CheckIfWareHousesHasActiveStatus(wareHouse);
            }).When(x => x.Type is MovementsTypesEnum.Transferencia)
                .WithMessage("One or both of the warehose are not active. ");
            RuleFor(x => x.WareHouse).Must((wareHouseMovement, _) =>
            {
                return !wareHouseMovementServices.WillStockFallBelowMinimum(wareHouseMovement);
            }).WithMessage("The stock level will fall below the minimum quantity allowed. ").When(x => x.Type is MovementsTypesEnum.Salida || x.Type is MovementsTypesEnum.Transferencia);
            RuleFor(x => x.WareHouse).Must((wareHouseMovement, _) =>
            {
                return !wareHouseMovementServices.WillStockFallMaximun(wareHouseMovement);
            }).When(x => x.Type is MovementsTypesEnum.Entrada ||
            x.Type is MovementsTypesEnum.Transferencia).WithMessage("The input quantity will exceed the maximum warehouse quantity. ");
            

        }
    }
}
