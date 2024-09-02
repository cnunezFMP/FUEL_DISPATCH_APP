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
            RuleFor(x => x.DispenserId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .WithMessage("El dispensador no se puede dejar vacio. ");

            RuleFor(x => x.Odometer)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .Must((x, _) => wareHouseMovementServices.CheckVehicleOdometer(x))
                .WithMessage("El odometro es menor o igual al del vehiculo. ");

            RuleFor(x => x.DriverId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .WithMessage("El conductor no se puede dejar vacio. ");

            RuleFor(x => x.VehicleId)
                .NotEmpty()
                .NotNull()
                .NotEqual(0)
                .WithMessage("El dispensador no se puede dejar vacio. ");

            RuleFor(x => x)
                .Must(wareHouseMovementServices.CheckVehicle)
                .WithMessage("El vehiculo es inactivo o no valido. ");


            RuleFor(x => x.Qty).Must((qty, _) => wareHouseMovementServices.QtyCantBeZero(qty))
                .WithMessage("La cantidad a despachar no puede ser cero. ");

            RuleFor(x => x)
                .Must(wareHouseMovementServices.CheckDriver)
                .WithMessage("El conductor esta inactivo o no es valido. ");

            RuleFor(x => x.BranchOffice)
                .Must((branch, _) =>
            {
                return !wareHouseMovementServices.CheckBranchOffice(branch);
            }).WithMessage("La sucursal no esta inactiva o no es valida. ");

            RuleFor(x => x.Dispenser)
                .Must((dispenser, _) => wareHouseMovementServices.CheckDispenser(dispenser))
                .WithMessage("El dispensador esta inactivo o no es valido. ");

            //RuleFor(x => x)
            //    .Must(wareHouseMovementServices.CheckIfProductIsInTheWareHouse)
            //    .When(x => x.Type is MovementsTypesEnum.Salida || x.Type is MovementsTypesEnum.Transferencia)
            //    .WithMessage("The product isn't in the specified warehouse. ");

            RuleFor(x => x)
                .Must((wareHouse, _) =>
            {
                return wareHouseMovementServices.CheckIfWareHousesHasActiveStatus(wareHouse);
            }).WithMessage("El almacen no esta activo. ");

            //RuleFor(x => x.WareHouse)
            //    .Must((wareHouseMovement, _) =>
            //{
            //    return !wareHouseMovementServices.WillStockFallBelowMinimum(wareHouseMovement);
            //}).WithMessage("The stock level will fall below the minimum quantity allowed. ");

            //RuleFor(x => x.WareHouse)
            //    .Must((wareHouseMovement, _) =>
            //{
            //    return !wareHouseMovementServices.WillStockFallMaximun(wareHouseMovement);
            //}).When(x => x.Type is MovementsTypesEnum.Entrada ||
            //x.Type is MovementsTypesEnum.Transferencia).WithMessage("The input quantity will exceed the maximum warehouse quantity. ");


        }
    }
}
