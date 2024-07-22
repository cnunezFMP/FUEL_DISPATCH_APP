using FMP_DISPATCH_API.Services.Emails;
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
    public class DriversServices : GenericRepository<Driver>, IDriversServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IEmailSender _emailSender;
        public DriversServices(FUEL_DISPATCH_DBContext dBContext, IEmailSender emailSender)
            : base(dBContext)
        {
            _DBContext = dBContext;
            _emailSender = emailSender;
        }
        public override ResultPattern<Driver> Post(Driver entity)
        {
            _DBContext.Driver.Add(entity);
            _DBContext.SaveChanges();
            if (entity.Email is not null)
                _emailSender.SendEmailAsync(entity.Email,
                    AppConstants.ACCOUNT_CREATED_MESSAGE,
                    "Your account was created successfully. ");

            return ResultPattern<Driver>.Success(
                entity,
                StatusCodes.Status200OK,
                "Driver added successfully. ");
        }
        public bool CheckIfIdIsUnique(Driver entity)
            => !_DBContext.Driver.Any(x => x.Identification == entity.Identification);
        // DONE: Chequear esta validacion. 
        public bool IsEmailUnique(Driver driver)
            => !_DBContext.User.Any(x => x.Email == driver.Email);

        /*public bool VehicleIdHasValue(Driver entity)
            => _DBContext.Vehicle.Any(x => x.Id == entity.VehicleId);
        public bool CheckAndUpdateDriver(Driver entity)
        {
            var vehicle = _DBContext.Vehicle.FirstOrDefault(x => x.Id == entity.VehicleId);

            if (vehicle!.DriverId!.HasValue)
                throw new BadRequestException("This vehicle has driver assigned. ");

            if (vehicle!.Status == ValidationConstants.InactiveStatus)
                throw new BadRequestException("This vehicle is inactive. ");

            vehicle.DriverId = entity.Id;
            _DBContext.Vehicle.Update(vehicle);
            _DBContext.SaveChanges();
            return true;
        }*/
        // DONE: Implementar esta funcion en el controlador de Driver
        public ResultPattern<List<WareHouseMovement>> GetDriverDispatches(int driverId)
        {
            var driverDispatches = _DBContext.WareHouseMovement
                .AsNoTracking()
                .Where(x => x.DriverId == driverId)
                .ToList()
                ?? throw new BadRequestException("This driver has no movements or, the vehicle doesn't exist. "); ;

            return ResultPattern<List<WareHouseMovement>>
                .Success
                (
                    driverDispatches,
                    StatusCodes.Status200OK,
                    "Driver dispatches obtained."
                );
        }
    }
}
