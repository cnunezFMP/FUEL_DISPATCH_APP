using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseMovementServices : GenericRepository<WareHouseMovement>, IWareHouseMovementServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public WareHouseMovementServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }
        // DONE: Hacer despachos y transferencias con solicitudes.
        public override ResultPattern<WareHouseMovement> Post(WareHouseMovement wareHouseMovement)
        {
            if (wareHouseMovement.RequestId.HasValue)
                SetRequestForMovement(wareHouseMovement);
            if (wareHouseMovement.VehicleId.HasValue)
            {
                SetDriverIdByVehicle(wareHouseMovement);
                VehicleHasMovements(wareHouseMovement);
                ChangeVehicleStatus(wareHouseMovement);
            }
            NoEnoughAmount(wareHouseMovement);
            ChangeDriverStatus(wareHouseMovement);
            _DBContext.WareHouseMovement.Add(wareHouseMovement);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouseMovement>.Success(wareHouseMovement, StatusCodes.Status201Created, "Registered dispatch. ");
        }
        #region Logic
        public bool SetDriverIdByVehicle(WareHouseMovement wareHouseMovement)
        {
            var vehicleDriver = _DBContext.Vehicle.AsNoTrackingWithIdentityResolution().FirstOrDefault(x => x.Id == wareHouseMovement.VehicleId);

            if (vehicleDriver!.DriverId is not null)
            {
                wareHouseMovement.DriverId = vehicleDriver!.DriverId;
                return true;
            }
            return false;
        }
        public bool VehicleHasMovements(WareHouseMovement wareHouseMovement)
        {
            var vehicleForDispatch = _DBContext.Vehicle
                .Include(x => x.WareHouseMovements)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == wareHouseMovement.VehicleId);

            if (vehicleForDispatch!.WareHouseMovements.Any())
            {
                if (!CheckPreviousVehicleDispatch(wareHouseMovement))
                {
                    throw new BadRequestException("El odometro no puede ser menor o igual al anterior de el vehiculo especificado. ");
                }
            }
            return false;
        }
        public bool QtyCantBeZero(WareHouseMovement wareHouseMovement)
            => wareHouseMovement.Qty > ValidationConstants.ZeroGallons;
        public bool CheckPreviousVehicleDispatch(WareHouseMovement wareHouseMovement)
        {
            var previousDispatch = _DBContext.WareHouseMovement
                .Where(x => x.VehicleId == wareHouseMovement.VehicleId)
                .OrderByDescending(x => x.WareHouseId)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault();

            return wareHouseMovement.Odometer > previousDispatch!.Odometer;
        }
        // DONE: Corregir las funciones "CheckVehicle", "CheckDriver".
        public bool CheckVehicle(int vehicleId)
        {
            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == vehicleId)
                                        ?? throw new NotFoundException("No se encontro el vehiculo. ");

            return vehicleForDispatch.Status is ValidationConstants.InactiveStatus || vehicleForDispatch!.Status is ValidationConstants.NotAvailableStatus;
        }
        public bool CheckDriver(int driverId)
        {

            var driverForDispatch = _DBContext.Driver
                .AsNoTrackingWithIdentityResolution()
                 .FirstOrDefault(d => d.Id == driverId)
                    ?? throw new NotFoundException("No driver found. ");


            return (driverForDispatch!.Status is ValidationConstants.InactiveStatus || driverForDispatch!.Status is ValidationConstants.NotAvailableStatus);

        }
        public bool CheckBranchOffice(WareHouseMovement wareHouseMovement)
        {
            var branchOffice = _DBContext.BranchOffices
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(b => b.Id == wareHouseMovement.BranchOfficeId)
                ?? throw new NotFoundException("No se encontro la sucursal para el despacho");

            return branchOffice.Status is ValidationConstants.InactiveStatus;
        }
        public bool CheckDispenser(WareHouseMovement wareHouseMovement)
        {
            var dispenser = _DBContext.Dispenser
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(dp => dp.Id == wareHouseMovement.DispenserId)
                ?? throw new NotFoundException("No se encontro dispensador para el despacho");

            return dispenser.Status is ValidationConstants.InactiveStatus;
        }
        public bool ChangeDriverStatus(WareHouseMovement wareHouseMovement)
        {

            var driverForDispatch = _DBContext.Driver
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(d => d.Id == wareHouseMovement.DriverId)
                    ?? throw new NotFoundException("No se encontro el conductor.");

            driverForDispatch.Status = ValidationConstants.NotAvailableStatus;

            return true;
        }
        public bool ChangeVehicleStatus(WareHouseMovement wareHouseMovement)
        {
            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == wareHouseMovement.VehicleId);

            vehicleForDispatch!.Status = ValidationConstants.NotAvailableStatus;

            return true;

        }
        public bool CheckWareHouseStock(WareHouseMovement wareHouseMovement)
        {
            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId);
            return wareHouseStock!.StockQty > 0 || wareHouseStock.StockQty > wareHouseMovement.Qty;
        }
        public bool CheckIfProductIsInTheWareHouse(WareHouseMovement wareHouseMovement)
            => _DBContext.vw_ActualStock
            .AsNoTrackingWithIdentityResolution()
            .Any(x => x.WareHouseId == wareHouseMovement!.WareHouseId && x.ItemId == wareHouseMovement!.ItemId);
        public bool CheckIfWareHousesHasActiveStatus(WareHouseMovement wareHouseMovement)
        {
            var wareHouse = _DBContext.WareHouse.AsNoTrackingWithIdentityResolution().FirstOrDefault(x => x.Id == wareHouseMovement.WareHouseId);
            var toWareHouse = _DBContext.WareHouse.AsNoTrackingWithIdentityResolution().FirstOrDefault(x => x.Id == wareHouseMovement.ToWareHouseId);
            if (wareHouseMovement.Type is "Transferencia")
                return wareHouse!.Status is ValidationConstants.ActiveStatus && toWareHouse!.Status is ValidationConstants.ActiveStatus;

            return wareHouse!.Status is ValidationConstants.ActiveStatus;
        }
        public bool WillStockFallBelowMinimum(WareHouseMovement wareHouseMovement)
        {
            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId);

            var wareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.WareHouseId);

            decimal currentQtyInWareHouse = wareHouseStock!.StockQty - wareHouseMovement.Qty;

            return currentQtyInWareHouse < wareHouse!.MinCapacity;
        }
        public bool WillStockFallMaximun(WareHouseMovement wareHouseMovement)
        {
            if (wareHouseMovement.Type is "Trasnferencia")
            {
                var toWareHouseStock = _DBContext.vw_ActualStock
                    .AsNoTrackingWithIdentityResolution()
                    .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.ToWareHouseId);

                var toWareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.ToWareHouseId);

                decimal currentQtyInToWareHouse = toWareHouseStock!.StockQty + wareHouseMovement.Qty;

                return currentQtyInToWareHouse > toWareHouse!.MaxCapacity;
            }

            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId);


            var wareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.WareHouseId);

            decimal currentQtyInWareHouse = wareHouseStock!.StockQty + wareHouseMovement.Qty;

            return currentQtyInWareHouse > wareHouse!.MaxCapacity;
        }
        public void NoEnoughAmount(WareHouseMovement wareHouseMovement)
        {
            var driverCurrentAmount = _DBContext.EmployeeConsumptionLimit
                .FirstOrDefault(x => x.DriverId == wareHouseMovement.DriverId
                && x.MethodOfComsuptionId == wareHouseMovement.FuelMethodOfComsuptionId)
                ?? throw new NotFoundException("This driver does not have this payment method or cannot be found. ");

            if (driverCurrentAmount!.MethodOfComsuptionId is ValidationConstants.CreditCardMethod)
            {
                if (wareHouseMovement.Amount > driverCurrentAmount.CurrentAmount)
                    throw new BadRequestException("Driver have'nt enough amount. ");

                var newDriverAmount = driverCurrentAmount.CurrentAmount - wareHouseMovement.Amount;
                driverCurrentAmount.CurrentAmount = newDriverAmount;
                _DBContext.SaveChanges();
            }

            if (driverCurrentAmount!.MethodOfComsuptionId is ValidationConstants.GallonsMethod)
            {
                if (wareHouseMovement.Qty > driverCurrentAmount.CurrentAmount)
                    throw new BadRequestException("Driver have'nt enough amount. ");

                var newDriverAmount = driverCurrentAmount.CurrentAmount - wareHouseMovement.Amount;
                driverCurrentAmount.CurrentAmount = newDriverAmount;
                _DBContext.SaveChanges();
            }
        }
        // DONE: Verificar el estado de las solicitudes. Agregar a FluentValidation.
        // TODO: Verificar si la validacion me funcionara en vez de las excepciones.
        public bool SetRequestForMovement(WareHouseMovement wareHouseMovement)
        {
            var requestForMovement = _DBContext.WareHouseMovementRequest.FirstOrDefault(x => x.Id == wareHouseMovement.RequestId)
                ?? throw new NotFoundException("No request found. ");

            /*if (requestForMovement.Status is ValidationConstants.PendingStatus)
                throw new BadRequestException($"This request is {ValidationConstants.PendingStatus}");

            if (requestForMovement.Status is ValidationConstants.RejectedStatus)
                throw new BadRequestException($"This request is {ValidationConstants.RejectedStatus}");

            if (requestForMovement.Status is ValidationConstants.CanceledStatus)
                throw new BadRequestException($"This request is {ValidationConstants.CanceledStatus}");*/

            wareHouseMovement.Qty = requestForMovement.Qty;
            wareHouseMovement.DriverId = requestForMovement.DriverId;
            wareHouseMovement.VehicleId = requestForMovement.VehicleId;
            return true;
        }
        #endregion
    }
}