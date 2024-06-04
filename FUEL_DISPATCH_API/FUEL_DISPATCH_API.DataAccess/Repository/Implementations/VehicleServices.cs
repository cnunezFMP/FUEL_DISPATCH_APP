using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class VehicleServices : GenericRepository<Vehicles>, IVehicleServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public VehicleServices(FUEL_DISPATCH_DBContext DBContext)
            : base(DBContext)
        {
            _DBContext = DBContext;
        }

        public override ResultPattern<Vehicles> Post(Vehicles entity)
        {
        }


        // En el caso de que se le quiera asignar conductor al vehiculo al momento de agregarlo.
        bool DriverIdHasValue(Vehicles entity)
        {
            if (entity.DriverId.HasValue)
            {
                if (!CheckIfDriverExists(entity))
                    throw new BadRequestException("This driver doesn't exists. ");

                var driver = _DBContext.Drivers.FirstOrDefault(x => x.Id == entity.DriverId);

                if(driver!.VehicleToken!.Any())
                    throw new BadRequestException("This driver has vehicle assigned. ");

                if (driver.Status == ValidationConstants.InactiveStatus)
                    throw new BadRequestException("This driver is inactive. ");


                _DBContext.Drivers.Update(driver);
                _DBContext.SaveChanges();
            }
        }

        bool CheckIfMeasureExists(Vehicles vehicle)
           => !_DBContext.Measures.Any(x => x.Id == vehicle.MeasureId);
        bool CheckIfMakeExists(Vehicles vehicle)
           => !_DBContext.Makes.Any(x => x.Id == vehicle.MakeId);

        bool CheckIfModelExists(Vehicles vehicle)
            => !_DBContext.Models.Any(x => x.Id == vehicle.ModelId);

        bool CheckIfGenerationExists(Vehicles vehicle)
            => !_DBContext.Generations.Any(x => x.Id == vehicle.GenerationId);

        bool CheckIfModEngineExists(Vehicles vehicle)
            => !_DBContext.ModEngine.Any(x => x.Id == vehicle.ModEngineId);

        bool CheckIfDriverExists(Vehicles vehicle)
            => _DBContext.Drivers.Any(x => x.Id == vehicle.DriverId);
    }
}
