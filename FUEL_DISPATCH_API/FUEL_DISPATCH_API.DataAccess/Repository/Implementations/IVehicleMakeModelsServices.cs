using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public interface IVehicleMakeModelsServices : IGenericInterface<VehiclesMakeModels>
    {
    }


    public class VehicleMakeModelsServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<VehiclesMakeModels>(DBContext, httpContextAccessor), IVehicleMakeModelsServices { }

}
