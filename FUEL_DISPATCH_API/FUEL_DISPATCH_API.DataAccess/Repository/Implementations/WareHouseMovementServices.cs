using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public override ResultPattern<WareHouseMovement> Post(WareHouseMovement wareHouseMovement)
        {
            SetDriverIdByVehicle(wareHouseMovement);

            if (!CheckVehicle(wareHouseMovement))
                throw new BadRequestException("Puede que el vehiculo no exista o que este inactivo. ");

            if (!CheckDriver(wareHouseMovement))
                throw new BadRequestException("Puede que el conductor no exista o que este inactivo. ");

            if (!CheckBranchOffice(wareHouseMovement))
                throw new BadRequestException("Puede que la sucursal no exista, o este inactiva. ");

            if (!CheckDispenser(wareHouseMovement))
                throw new BadRequestException("Puede que el dispensador no exista, o este inactiva. ");

            if (!CheckPreviousDispatch(wareHouseMovement))
                throw new BadRequestException("El odometro no puede ser menor ni igual al anterior de este vehiculo.");

            _DBContext.WareHouseMovement.Add(wareHouseMovement);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouseMovement>.Success(wareHouseMovement,
                                                            StatusCodes.Status201Created,
                                                            "Despacho registrado. ");
        }

        public bool SetDriverIdByVehicle(WareHouseMovement wareHouseMovement)
        {
            var vehicleDriver = _DBContext.Vehicle.Find(wareHouseMovement.VehicleId);
            if (vehicleDriver is not null && vehicleDriver.DriverId is not null)
            {
                wareHouseMovement.DriverId = vehicleDriver!.DriverId;
                return true;
            }
            return false;
        }

        public bool QtyCantBeZero(WareHouseMovement wareHouseMovement)
            => wareHouseMovement.Qty is not ValidationConstants.ZeroGallons;
        public bool CheckPreviousDispatch(WareHouseMovement wareHouseMovement)
        {
            if (wareHouseMovement.VehicleId.HasValue)
            {
                var previousDispatch = _DBContext.WareHouseMovement
                                   .Where(x => x.VehicleId == wareHouseMovement.VehicleId)
                                   .OrderByDescending(x => x.Id)
                                   .FirstOrDefault();
                if (previousDispatch is null)
                    return false;

                return wareHouseMovement.Odometer > previousDispatch.Odometer;
            }
            return false;
        }
        public bool CheckVehicle(WareHouseMovement wareHouseMovement)
        {
            if (wareHouseMovement.VehicleId.HasValue)
            {
                var vehicleForDispatch = _DBContext.Vehicle
                                            .AsNoTrackingWithIdentityResolution()
                                            .FirstOrDefault(v => v.Id == wareHouseMovement.VehicleId)
                        ?? throw new NotFoundException("No se encontro el vehiculo para el despacho");

                return vehicleForDispatch is not null && vehicleForDispatch.Status is not ValidationConstants.InactiveStatus;
            }
            return true;
        }
        public bool CheckDriver(WareHouseMovement wareHouseMovement)
        {
            if (wareHouseMovement.DriverId.HasValue)
            {
                var driverForDispatch = _DBContext.Driver
                                            .AsNoTrackingWithIdentityResolution()
                                            .FirstOrDefault(d => d.Id == wareHouseMovement.DriverId)
                        ?? throw new NotFoundException("No se encontro el conductor para el despacho");

                return driverForDispatch is not null &&
                       driverForDispatch!.Status is not ValidationConstants.InactiveStatus;
            }
            return false;
        }
        public bool CheckBranchOffice(WareHouseMovement wareHouseMovement)
        {
            var branchOffice = _DBContext.BranchOffices
                               .AsNoTrackingWithIdentityResolution()
                               .FirstOrDefault(b => b.Id == wareHouseMovement.BranchOfficeId)
                ?? throw new NotFoundException("No se encontro la sucursal para el despacho");
            return branchOffice is not null &&
                   branchOffice.Status is not ValidationConstants.InactiveStatus;
        }
        public bool CheckDispenser(WareHouseMovement wareHouseMovement)
        {
            var dispenser = _DBContext.Dispenser
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(dp => dp.Id == wareHouseMovement.DispenserId)
                ?? throw new NotFoundException("No se encontro dispensador para el despacho");
            return dispenser is not null &&
                   dispenser.Status is not ValidationConstants.InactiveStatus;
        }
    }
}
