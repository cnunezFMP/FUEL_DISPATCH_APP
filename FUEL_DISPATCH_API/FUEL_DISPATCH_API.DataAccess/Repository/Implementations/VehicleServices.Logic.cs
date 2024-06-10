using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public partial class VehiclesServices : GenericRepository<Vehicle>, IVehiclesServices
    {
        bool DriverIdHasValue(Vehicle entity)
        {
            if (!CheckIfDriverExists(entity))
                throw new NotFoundException("This driver doesn't exists. ");
            return true;
        }
        bool CheckAndUpdateDriver(Vehicle entity)
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
        bool CheckIfMeasureExists(Vehicle vehicle)
           => !_DBContext.Measure.Any(x => x.Id == vehicle.MeasureId);
        bool CheckIfMakeExists(Vehicle vehicle)
           => !_DBContext.Make.Any(x => x.Id == vehicle.MakeId);
        bool CheckIfModelExists(Vehicle vehicle)
            => !_DBContext.Model.Any(x => x.Id == vehicle.ModelId);
        bool CheckIfGenerationExists(Vehicle vehicle)
            => !_DBContext.Generation.Any(x => x.Id == vehicle.GenerationId);
        bool CheckIfModEngineExists(Vehicle vehicle)
            => !_DBContext.ModEngine.Any(x => x.Id == vehicle.ModEngineId);
        bool CheckIfDriverExists(Vehicle vehicle)
            => _DBContext.Driver.Any(x => x.Id == vehicle.DriverId);
    }
}
