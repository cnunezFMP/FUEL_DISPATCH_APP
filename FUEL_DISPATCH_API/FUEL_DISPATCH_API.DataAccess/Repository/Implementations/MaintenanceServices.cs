using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Twilio.TwiML.Voice;
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
            SetCurrentOdometerByVehicle(entity);
            SetNextMaintenanceOdometer(entity);
            SetVehicleStatus(entity);
            _DBContext.Maintenance.Add(entity);
            _DBContext.SaveChanges();
            SetNextMaintenanceDate(entity);
            return ResultPattern<Maintenance>.Success(entity!,
                StatusCodes.Status201Created,
                AppConstants.DATA_SAVED_MESSAGE);
        }
        public override ResultPattern<Maintenance> Update(Func<Maintenance, bool> predicate, Maintenance updatedEntity)
        {
            SetCurrentOdometerByVehicle(updatedEntity);
            SetNextMaintenanceOdometer(updatedEntity);
            SetVehicleStatus(updatedEntity);
            SetVehicleStatusToActive(updatedEntity);
            _DBContext.Entry(updatedEntity).CurrentValues.SetValues(updatedEntity);
            _DBContext.Update(updatedEntity);
            _DBContext.SaveChanges();
            SetNextMaintenanceDate(updatedEntity);
            return ResultPattern<Maintenance>.Success(updatedEntity,
                StatusCodes.Status200OK,
                AppConstants.DATA_UPDATED_MESSAGE);
        }
        public bool SetCurrentOdometerByVehicle(Maintenance maintenance)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?
                .Items["CompanyId"]?
                .ToString();
            branchId = _httpContextAccessor.HttpContext?
                .Items["BranchOfficeId"]?
                .ToString();*/

            var vehicleForMaintenance = _DBContext.Vehicle
                .FirstOrDefault(x => x.Id == maintenance.VehicleId /*&&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId)*/) ??
                throw new NotFoundException("No vehicle find for this id. ");

            maintenance.VehicleVin = vehicleForMaintenance.VIN;
            maintenance.CurrentOdometer = vehicleForMaintenance.Odometer;
            return true;
        }
        public bool SetNextMaintenanceDate(Maintenance maintenance)
        {
            /*string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString() ??
           throw new BadRequestException("Invalid company. ");*/

            var part = _DBContext.Part
                .FirstOrDefault(x => x.Id == maintenance.PartId /*&&
                x.CompanyId == int.Parse(companyId)*/) ??
                throw new NotFoundException("La pieza no se encuentra registrada en la compañia");

            var dateForNextMaintenance = maintenance.CreatedAt.AddMonths(part!.MaintenanceMonthsInt);

            maintenance.NextMaintenanceDate = dateForNextMaintenance;
            // Actualizar la fecha de la proxima mantencion

            _DBContext.Maintenance.Update(maintenance);
            _DBContext.SaveChanges();

            return true;
        }
        public bool SetNextMaintenanceOdometer(Maintenance maintenance)
        {
            /*string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?.Items["CompanyId"]?.ToString() ??
                throw new BadRequestException("Invalid company. ");*/

            var part = _DBContext.Part
                .FirstOrDefault(x => x.Id == maintenance.PartId/* &&
                x.CompanyId == int.Parse(companyId)*/) ??
                throw new NotFoundException("Piece is not registered in the company. ");

            maintenance.PartCode = part.Code;
            maintenance.OdometerUpcomingMaintenance = maintenance.CurrentOdometer + part!.MaintenanceOdometerInt;
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
        public bool SetVehicleStatusToActive(Maintenance maintenance)
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
                .FirstOrDefault(/*x => x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId) &&*/
                x.Id == maintenance.VehicleId) ??
                throw new NotFoundException("Vehiculo especificado no encontrado. ");

            if (maintenance.Status is Enums.MaitenanceStatusEnum.Canceled ||
                maintenance.Status is Enums.MaitenanceStatusEnum.Completed ||
                maintenance.Status is Enums.MaitenanceStatusEnum.NotStarted)
                vehicle.Status = Enums.VehicleStatussesEnum.Active;

            return true;
        }
    }
}
