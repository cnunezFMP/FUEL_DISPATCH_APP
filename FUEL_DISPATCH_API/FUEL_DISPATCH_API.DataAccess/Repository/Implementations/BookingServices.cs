using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class BookingServices : GenericRepository<Booking>, IBookingServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IWareHouseMovementServices _wareHouseServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingServices(FUEL_DISPATCH_DBContext dbContext, IWareHouseMovementServices wareHouseServices, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
            _wareHouseServices = wareHouseServices;
        }
        // DONE: Hacer este servicio para Booking. 
        public bool CheckDriver(int driverId)
        {
            var driverForBook = _DBContext.Driver
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(d => d.Id == driverId)
                ?? throw new NotFoundException("No driver found. ");

            return (driverForBook!.Status is not ValidationConstants.InactiveStatus
                && driverForBook!.Status is not ValidationConstants.NotAvailableStatus);
        }
        public bool CheckVehicle(Booking booking)
            => _wareHouseServices.CheckVehicle(booking.VehicleId);
        public bool VerifyDisponibility(Booking booking)
            => !_DBContext.Booking.Any(r => r.VehicleId == booking.VehicleId
                       && r.Status != ValidationConstants.CanceledStatus
                       && (booking.SpecificDate <= r.ToSpecificDate
                       && booking.ToSpecificDate >= r.SpecificDate)
                       && r.SpecificDate != booking.SpecificDate);
        public bool VehicleHasDriverAssigned(Booking booking)
        {
            var vehicleForBook = _DBContext.Vehicle.FirstOrDefault(x => x.Id == booking.VehicleId);
            return !vehicleForBook!.DriverId!.HasValue;
        }
    }
}
