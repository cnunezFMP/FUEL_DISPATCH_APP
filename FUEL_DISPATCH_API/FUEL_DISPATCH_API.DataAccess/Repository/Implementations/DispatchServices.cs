using FMP_DISPATCH_API.Services.Emails;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DispatchServices : GenericRepository<Dispatch>, IDispatchServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;
        public DispatchServices(IEmailSender emailSender, FUEL_DISPATCH_DBContext dBContext)
            : base(dBContext)
        {
            _emailSender = emailSender;
            _DBContext = dBContext;
        }
        public override ResultPattern<Dispatch> Post(Dispatch entity)
        {

            if (!CheckDispatch(entity))
                throw new BadRequestException("Revise que el odometro registrado no sea igual o menor al anterior." +
                                              "Tambien, que la cantidad de combustible digitados no esten en cero. ");
            if (!CheckDriver(entity))
                throw new BadRequestException("Puede que el conductor no exista o que este inactivo. ");

            if (!CheckVehicle(entity))
                throw new BadRequestException("Puede que el vehiculo no exista, o este inactivo. ");

            if (!CheckBranchOffice(entity))
                throw new BadRequestException("Puede que la sucursal no exista, o este inactiva. ");

            if (!CheckDispenser(entity))
                throw new BadRequestException("Puede que el dispensador no exista, o este inactiva. ");

            _DBContext.Dispatch.Add(entity);
            _DBContext.SaveChanges();
            return ResultPattern<Dispatch>.Success(entity, StatusCodes.Status201Created, "Despacho creado. ");
        }
        bool CheckDispatch(Dispatch dispatch)
        {
            var previousDispatch = _DBContext.Dispatch.Where(x => x.VehicleId == dispatch.VehicleId).OrderByDescending(x => x.Id).FirstOrDefault();
            return dispatch.Gallons is not ValidationConstants.ZeroGallons &&
                   dispatch.Odometer > previousDispatch!.Odometer;
        }
        bool CheckVehicle(Dispatch dispatch)
        {
            var vehicleForDispatch = _DBContext.Vehicle.FirstOrDefault(x => x.Id == dispatch.VehicleId);
            return vehicleForDispatch is not null &&
                   vehicleForDispatch!.Status is not ValidationConstants.InactiveStatus;
        }
        bool CheckDriver(Dispatch dispatch)
        {
            var driverForDispatch = _DBContext.Driver.Find(dispatch.DriverId);
            return driverForDispatch is not null &&
                   driverForDispatch!.Status is not ValidationConstants.InactiveStatus;
        }
        bool CheckBranchOffice(Dispatch dispatch)
        {
            var branchOffice = _DBContext.BranchOffices.FirstOrDefault(x => x.Id == dispatch.Id);
            return branchOffice is not null &&
                   branchOffice.Status is not ValidationConstants.InactiveStatus;
        }
        bool CheckDispenser(Dispatch dispatch)
        {
            var dispenser = _DBContext.Dispenser.FirstOrDefault(x => x.Id == dispatch.Id);
            return dispenser is not null &&
                   dispenser.Status is not ValidationConstants.InactiveStatus;
        }
    }
}
