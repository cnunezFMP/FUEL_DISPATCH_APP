using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Components.Forms;
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

        public override ResultPattern<Maintenance> Post(Maintenance entity)
        {
            foreach (var details in entity.Details)
                SetNextMaintenanceDate(details);

            SetCurrentOdometerByVehicle(entity);
            SetNextMaintenanceOdometer(entity);
            SetVehicleStatus(entity);

            return base.Post(entity);
        }

        public bool SetNextMaintenanceDate(MaintenanceDetails maintenanceDetails)
        {
            /*string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString() ??
           throw new BadRequestException("Invalid company. ");*/

            var part = _DBContext.Part
                .FirstOrDefault(x => x.Id == maintenanceDetails.PartId /*&&
                x.CompanyId == int.Parse(companyId)*/) ??
                throw new NotFoundException("La pieza no se encuentra registrada en la compañia");

            var dateForNextMaintenance = maintenanceDetails.CreatedAt.AddMonths(part!.MaintenanceMonthsInt);

            maintenanceDetails.NextMaintenanceDate = dateForNextMaintenance;
            return true;
        }
        public bool SetCurrentOdometerByVehicle(Maintenance maintenanceHeader)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?
                .Items["CompanyId"]?
                .ToString();
            branchId = _httpContextAccessor.HttpContext?
                .Items["BranchOfficeId"]?
                .ToString();*/

            var vehicleForMaintenance = _DBContext.Vehicle
                .FirstOrDefault(x => x.Id == maintenanceHeader.VehicleId /*&&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId)*/) ??
                throw new NotFoundException("No vehicle find for this id. ");

            maintenanceHeader.VehicleVin = vehicleForMaintenance.VIN;
            maintenanceHeader.CurrentOdometer = vehicleForMaintenance.Odometer;
            return true;
        }
        public bool SetNextMaintenanceOdometer(Maintenance maintenance)
        {
            /*string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?.Items["CompanyId"]?.ToString() ??
                throw new BadRequestException("Invalid company. ");*/

            foreach (var detail in maintenance.Details)
            {
                var part = _DBContext.Part
                .FirstOrDefault(x => x.Id == detail.PartId/* &&
                x.CompanyId == int.Parse(companyId)*/) ??
                throw new NotFoundException("Piece is not registered in the company. ");

                // DONE: Resolver el problema del nulo aqui.
                detail.PartCode = part.Code;

                var upcomingMaintenanceOdometer = maintenance.CurrentOdometer + part!.MaintenanceOdometerInt;

                detail.OdometerUpcomingMaintenance = upcomingMaintenanceOdometer;
            }
            return true;
        }
        public bool SetVehicleStatus(Maintenance maintenance)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?
                .Items["CompanyId"]?
                .ToString();

            branchId = _httpContextAccessor.HttpContext?
                .Items["BranchOfficeId"]?
                .ToString();*/

            var vehicle = _DBContext
                .Vehicle
                .FirstOrDefault(x => /*x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId) &&*/
                x.Id == maintenance.VehicleId) ??
                throw new NotFoundException("Vehiculo especificado no encontrado. ");

            if (maintenance.Status is Enums.MaitenanceStatusEnum.InProgress)
                vehicle.Status = Enums.VehicleStatussesEnum.NotAvailable;

            return true;
        }
    }
}
