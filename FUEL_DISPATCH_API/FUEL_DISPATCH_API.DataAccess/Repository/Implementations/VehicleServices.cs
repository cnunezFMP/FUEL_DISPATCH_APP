using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class VehiclesServices : GenericRepository<Vehicle>, IVehiclesServices
    {
        public readonly FUEL_DISPATCH_DBContext _DBContext;
        public VehiclesServices(FUEL_DISPATCH_DBContext DBContext)
            : base(DBContext)
        {
            _DBContext = DBContext;
        }
        public override ResultPattern<Vehicle> Post(Vehicle entity)
        {
            if (CheckIfMeasureExists(entity))
                throw new NotFoundException("The measure doesn't exists. ");

            if (CheckIfMakeExists(entity))
                throw new NotFoundException("Make not found. ");

            if (CheckIfModelExists(entity))
                throw new NotFoundException("Model not found. ");

            if (CheckIfGenerationExists(entity))
                throw new NotFoundException("Generation not found. ");

            if (CheckIfModEngineExists(entity))
                throw new NotFoundException("Generation not found. ");

            DriverIdHasValue(entity);

            _DBContext.Vehicle.Add(entity);
            _DBContext.SaveChanges();
            if (DriverIdHasValue(entity))
                CheckAndUpdateDriver(entity);

            return ResultPattern<Vehicle>.Success(entity, StatusCodes.Status201Created, "Vehicle created successfully. ");
        }

        public bool DriverIdHasValue(Vehicle entity)
            => _DBContext.Driver.Any(x => x.Id == entity.DriverId);
        public bool CheckAndUpdateDriver(Vehicle entity)
        {
            var driver = _DBContext.Driver.FirstOrDefault(x => x.Id == entity.DriverId);

            if (driver!.VehicleId!.HasValue)
                throw new BadRequestException("This driver has vehicle assigned. ");

            if (driver!.Status == ValidationConstants.InactiveStatus)
                throw new BadRequestException("This driver is inactive. ");

            driver.VehicleId = entity.Id;
            _DBContext.Driver.Update(driver);
            _DBContext.SaveChanges();
            return true;
        }
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
            => !_DBContext.Vehicle.Any(x => x.Ficha == vehicleToken.Ficha);

        // DONE: Implementar esto en el controlador de Vehicle
        // TODO: Ver si implementar la misma forma de devolver los despachos como los "BranchOffices" en las compañias. 
        public ResultPattern<List<WareHouseMovement>> GetVehicleDispatches(int vehicleId)
        {
            var driverDispatches = _DBContext.WareHouseMovement
                .AsNoTracking()
                .Where(x => x.VehicleId == vehicleId)
                .ToList()
                ?? throw new BadRequestException("This vehicle has no movements or, the vehicle doesn't exist. ");

            return ResultPattern<List<WareHouseMovement>>
                .Success
                (
                    driverDispatches,
                    StatusCodes.Status200OK,
                    "Vehicle dispatches obtained."
                );
        }
    }
}
