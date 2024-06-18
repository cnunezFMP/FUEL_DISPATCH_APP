using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

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
        public override ResultPattern<WareHouseMovement> Post(WareHouseMovement wareHouse)
        {
            if (!CheckDispatch(wareHouse))
                throw new BadRequestException("Revise que el odometro registrado no sea igual o menor al anterior." +
                                              "Tambien, que la cantidad de combustible digitados no esten en cero. ");
            if (!CheckDriver(wareHouse))
                throw new BadRequestException("Puede que el conductor no exista o que este inactivo. ");

            if (!CheckVehicle(wareHouse))
                throw new BadRequestException("Puede que el vehiculo no exista, o este inactivo. ");

            if (!CheckBranchOffice(wareHouse))
                throw new BadRequestException("Puede que la sucursal no exista, o este inactiva. ");

            if (!CheckDispenser(wareHouse))
                throw new BadRequestException("Puede que el dispensador no exista, o este inactiva. ");

            _DBContext.WareHouseMovement.Add(wareHouse);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouseMovement>.Success(wareHouse, StatusCodes.Status201Created, "Despacho registrado. ");
        }
        
        bool CheckDispatch(WareHouseMovement dispatch)
        {
            var previousDispatch = _DBContext.WareHouseMovement.Where(x => x.VehicleId == dispatch.VehicleId).OrderByDescending(x => x.Id).FirstOrDefault();
            return dispatch.Qty is not ValidationConstants.ZeroGallons &&
                   dispatch.Odometer > previousDispatch!.Odometer;
        }
        bool CheckVehicle(WareHouseMovement dispatch)
        {
            var vehicleForDispatch = _DBContext.Vehicle.FirstOrDefault(x => x.Id == dispatch.VehicleId);
            return vehicleForDispatch is not null &&
                   vehicleForDispatch!.Status is not ValidationConstants.InactiveStatus;
        }
        bool CheckDriver(WareHouseMovement dispatch)
        {
            var driverForDispatch = _DBContext.Driver.Find(dispatch.DriverId);
            return driverForDispatch is not null &&
                   driverForDispatch!.Status is not ValidationConstants.InactiveStatus;
        }
        bool CheckBranchOffice(WareHouseMovement dispatch)
        {
            var branchOffice = _DBContext.BranchOffices.FirstOrDefault(x => x.Id == dispatch.Id);
            return branchOffice is not null &&
                   branchOffice.Status is not ValidationConstants.InactiveStatus;
        }
        bool CheckDispenser(WareHouseMovement dispatch)
        {
            var dispenser = _DBContext.Dispenser.Find(wareHouseMovement.DispenserId);
            return dispenser is not null &&
                   dispenser.Status is not ValidationConstants.InactiveStatus;
        }

    }
}
