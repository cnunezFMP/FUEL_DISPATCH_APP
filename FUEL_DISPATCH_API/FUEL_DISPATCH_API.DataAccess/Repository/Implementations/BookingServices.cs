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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        // DONE: Hacer este servicio para Booking. 
        public bool CheckDriver(Booking book)
        {
            string? branchOfficeId = _httpContextAccessor.HttpContext?.User.FindFirst("BranchOfficeId")?.Value;

            var driverForBook = _DBContext.Driver
                .AsNoTrackingWithIdentityResolution()
                .Where(d => d.BranchOfficeId == int.Parse(branchOfficeId))
                .FirstOrDefault(d => d.Id == book.DriverId)
                ?? throw new NotFoundException("No driver found. ");

            return (driverForBook!.Status is not ValidationConstants.InactiveStatus
                && driverForBook!.Status is not ValidationConstants.NotAvailableStatus);
        }
        public bool CheckVehicle(Booking book)
        {
            string? companyId, branchOfficeId;
            // DONE: Utilizar httpContextAccessor para obtener el companyId y branchOfficeId.
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == book.VehicleId &&
                v.CompanyId == int.Parse(companyId) &&
                v.BranchOfficeId == int.Parse(branchOfficeId))
                ?? throw new NotFoundException("No vehicle found. ");

            return (vehicleForDispatch.Status is not ValidationConstants.InactiveStatus
                && vehicleForDispatch!.Status is not ValidationConstants.NotAvailableStatus);
        }
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
