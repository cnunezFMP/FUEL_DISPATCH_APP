using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.TwiML.Voice;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class BookingServices : GenericRepository<Booking>, IBookingServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IWareHouseMovementServices _wareHouseServices;
        public BookingServices(FUEL_DISPATCH_DBContext dbContext, IWareHouseMovementServices wareHouseServices) : base(dbContext)
        {
            _DBContext = dbContext;
            _wareHouseServices = wareHouseServices;
        }
        public bool CheckDriver(Booking booking)
        {
            return _wareHouseServices.CheckDriver(booking.DriverId);
        }
        public bool CheckVehicle(Booking booking)
        {
            return _wareHouseServices.CheckVehicle(booking.VehicleId);
        }
        public bool VerifyDisponibility(Booking booking)
        {
            return !_DBContext.Booking.Any(r => r.VehicleId == booking.VehicleId
                       && r.Status != ValidationConstants.BookingCanceledStatus
                       && (booking.SpecificDate <= r.ToSpecificDate && booking.ToSpecificDate >= r.SpecificDate)
                       && r.SpecificDate != booking.SpecificDate);
        }
        public bool VehicleHasDriverAssigned(Booking booking)
        {
            var vehicleForBook = _DBContext.Vehicle.FirstOrDefault(x => x.Id == booking.VehicleId);
            return !vehicleForBook!.DriverId!.HasValue;
        }
    }
}
