using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class VehiclesServices : GenericRepository<Vehicle>, IVehiclesServices
    {
        public readonly FUEL_DISPATCH_DBContext _DBContext;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public VehiclesServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor)
            : base(DBContext, httpContextAccessor)
        {
            _DBContext = DBContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ResultPattern<Vehicle> Post(Vehicle entity)
        {
            if (CheckIfMeasureExists(entity))
                throw new NotFoundException("La medida no existe. ");

            if (CheckIfMakeExists(entity))
                throw new NotFoundException("No se encuentra la marca. ");

            if (CheckIfModelExists(entity))
                throw new NotFoundException("No se encuentra el modelo . ");

            if (CheckIfGenerationExists(entity))
                throw new NotFoundException("No se encuentra la generacion. ");
            if (CheckIfModEngineExists(entity))
                throw new NotFoundException("No se encuentra el motor. ");

            if (FichaMustBeUnique(entity))
                throw new BadRequestException("Existe un vehiculo con esta ficha asignada. ");
            //DriverIdHasValue(entity);

            return base.Post(entity);
        }

        /*public bool DriverIdHasValue(Vehicle entity)
            => _DBContext.Driver.Any(x => x.Id == entity.DriverId);*/
        public bool CheckIfMakeExists(Vehicle vehicle)
           => !_DBContext.Make.Any(x => x.Id == vehicle.MakeId);
        public bool CheckIfModelExists(Vehicle vehicle)
            => !_DBContext.Model.Any(x => x.Id == vehicle.ModelId);
        public bool CheckIfGenerationExists(Vehicle vehicle)
            => !_DBContext.Generation.Any(x => x.Id == vehicle.GenerationId);
        public bool CheckIfModEngineExists(Vehicle vehicle)
            => !_DBContext.ModEngine.Any(x => x.Id == vehicle.ModEngineId);
        public bool CheckIfMeasureExists(Vehicle vehicle)
            => !_DBContext.Measure.Any(x => x.Id == vehicle.OdometerMeasureId);
        public bool FichaMustBeUnique(Vehicle vehicleToken)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            return _DBContext.Vehicle
                .Any(x => x.Ficha == vehicleToken.Ficha /*&&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId)*/);
        }
        // DONE: Implementar esto en el controlador de Vehicle
        public ResultPattern<List<WareHouseMovement>> GetVehicleDispatches(int vehicleId)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/
            // DONE: Simplificar
            var driverDispatches = _DBContext.WareHouseMovement.
                                    /*join t1 in _DBContext.BranchOffices on t0.BranchOfficeId equals int.Parse(branchId)
                                    join t2 in _DBContext.Companies on t1.CompanyId equals int.Parse(companyId)*/
                                    Where(t0 => t0.VehicleId == vehicleId)
                                    .ToList();



            if (driverDispatches.Count == 0)
                throw new BadRequestException("El vehiculo no tiene movimientos previos. ");

            return ResultPattern<List<WareHouseMovement>>
                .Success(
                driverDispatches,
                StatusCodes.Status200OK,
                "Despachos del vehiculo obtenido. ");
        }

    }
}