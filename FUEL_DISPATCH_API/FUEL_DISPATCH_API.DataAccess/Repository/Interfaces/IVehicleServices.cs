using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IVehiclesServices : IGenericInterface<Vehicle>
    {
        bool DriverIdHasValue(Vehicle entity);
        bool CheckAndUpdateDriver(Vehicle entity);
        bool CheckIfMakeExists(Vehicle vehicle);
        bool CheckIfModelExists(Vehicle vehicle);
        bool CheckIfGenerationExists(Vehicle vehicle);
        bool CheckIfModEngineExists(Vehicle vehicle);
        bool CheckIfMeasureExists(Vehicle vehicle);
        bool TokenMustBeUnique(Vehicle vehicleToken);
    }
}
