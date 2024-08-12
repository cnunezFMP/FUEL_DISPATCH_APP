using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class MaintenanceServices : GenericRepository<Maintenance>, IMaintenanceServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MaintenanceServices(FUEL_DISPATCH_DBContext dbContext,
                                   IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool SetCurrentOdometerByVehicle(Maintenance maintenance)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var vehicleForMaintenance = _DBContext.Vehicle
                .FirstOrDefault(x => x.Id == maintenance.VehicleId &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId))
                ?? throw new NotFoundException("No vehicle find for this id. ");

            maintenance.CurrentOdometer = vehicleForMaintenance.Odometer;

            return true;
        }
        public bool SetNextMaintenanceDate(Maintenance maintenance)
        {
            string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString() ??
           throw new BadRequestException("Invalid company. ");

            var part = _DBContext.Part
                .FirstOrDefault(x => x.Id == maintenance.PartId &&
                x.CompanyId == int.Parse(companyId)) ??
                throw new NotFoundException("La pieza no se encuentra registrada en la compañia");

            maintenance.NextMaintenanceDate = maintenance.CreatedAt.AddMonths(part!.MaintenanceMonthsInt);
            return true;
        }
        public bool SetNextMaintenanceOdometer(Maintenance maintenance)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString() ??
                throw new BadRequestException("Invalid company. ");

            var part = _DBContext.Part
                .FirstOrDefault(x => x.Id == maintenance.PartId &&
                x.CompanyId == int.Parse(companyId)) ??
                throw new NotFoundException("La pieza no se encuentra registrada en la compañia");


            maintenance.OdometerUpcomingMaintenance = maintenance.CurrentOdometer + part!.MaintenanceOdometerInt;
            return true;
        }
    }
}
