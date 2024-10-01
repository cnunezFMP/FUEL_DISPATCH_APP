using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class MaintenanceServices(FUEL_DISPATCH_DBContext dbContext,
                               IHttpContextAccessor httpContextAccessor,
                               IWebHostEnvironment webHostEnvironment) : GenericRepository<Maintenance>(dbContext, httpContextAccessor), IMaintenanceServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext = dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        public override ResultPattern<Maintenance> Post(Maintenance entity)
        {
            if (!IsVehicleActive(entity))
                throw new BadRequestException("Este vehiculo no esta activo. ");


            foreach (var details in entity.Details)
                SetNextMaintenanceDate(details);

            SetCurrentOdometerByVehicle(entity);
            SetNextMaintenanceOdometer(entity);
            SetVehicleStatus(entity);

            return base.Post(entity);
        }
        public override ResultPattern<Maintenance> Update(Func<Maintenance, bool> predicate, Maintenance updatedEntity)
        {
            //RemoveDetails(predicate, updatedEntity);
            //AddDetails(predicate, updatedEntity);
            var maintenanceUpdate = _DBContext
                .Maintenance
                .Include(x => x.Details)
                .FirstOrDefault(predicate) ??
                throw new NotFoundException("El mantenimiento no se encontro. ");

            var lines = maintenanceUpdate.Details.ToList();

            lines.ForEach(x =>
            {
                var line = updatedEntity.Details.SingleOrDefault(y => y.MaintenanceId == x.MaintenanceId && y.Id == x.Id);

                if (line is null)
                    _DBContext.MaintenanceDetails.Remove(x);
                else
                    _DBContext.MaintenanceDetails.Entry(x).CurrentValues.SetValues(line);

            });

            var newLines = updatedEntity.Details.Where(x => x.Id == 0).ToList();

            newLines.ForEach(x =>
            {
                x.MaintenanceId = maintenanceUpdate.Id;
                _DBContext.MaintenanceDetails.Add(x);
            });

            foreach (var details in updatedEntity.Details)
                SetNextMaintenanceDate(details);

            SetCurrentOdometerByVehicle(updatedEntity);
            SetNextMaintenanceOdometer(updatedEntity);
            SetVehicleStatus(updatedEntity);

            return base.Update(predicate, updatedEntity);
        }
        public bool SetNextMaintenanceDate(MaintenanceDetails maintenanceDetails)
        {
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

            if (maintenance.Status is Enums.MaitenanceStatusEnum.Canceled || maintenance.Status is Enums.MaitenanceStatusEnum.Completed)
                vehicle.Status = Enums.VehicleStatussesEnum.Active;

            return true;
        }

        public ResultPattern<string> UploadAnexo(IFormFile file, int maintenanceId)
        {
            var maintenance = _DBContext.Maintenance.Find(maintenanceId) ??
                throw new NotFoundException("No se encontro el mantenimiento. ");

            var directory = Path.Combine(_webHostEnvironment.WebRootPath, "anexos");

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            string? fileName = Guid.NewGuid().ToString("N");

            var ext = Path.GetExtension(file.FileName);

            fileName += ext;

            var filePath = Path.Combine(directory, fileName);

            using var ms = new MemoryStream();

            file.CopyToAsync(ms).Wait();

            File.WriteAllBytes(filePath, ms.ToArray());

            var relativePath = filePath.Replace(_webHostEnvironment.WebRootPath, string.Empty);

            _DBContext.AnexosMantenimientos.Add(new AnexoMantenimiento { MaintenanceId = maintenance.Id, Ruta = relativePath });
            _DBContext.SaveChanges();

            return ResultPattern<string>.Success(relativePath, 201, "Anexo cargado. ");
        }

        public bool IsVehicleActive(Maintenance maintenance)
        {
            var vehicle = _DBContext.Vehicle.Find(maintenance.VehicleId);

            return (vehicle!.Status is not Enums.VehicleStatussesEnum.Inactive &&
                    vehicle.Status is not Enums.VehicleStatussesEnum.NotAvailable);
        }
    }
}
