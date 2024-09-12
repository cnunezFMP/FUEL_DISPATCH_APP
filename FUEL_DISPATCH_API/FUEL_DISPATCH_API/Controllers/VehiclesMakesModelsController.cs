using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class VehiclesMakesModelsController(IVehicleMakeModelsServices vehicleMakeModelsServices) : ControllerBase
    {
        private readonly IVehicleMakeModelsServices _vehicleMakeModelsServices = vehicleMakeModelsServices;

        [HttpGet]
        public ActionResult<ResultPattern<Paging<VehiclesMakeModels>>> GetVehiclesMakeModels([FromQuery] GridifyQuery query)
             => Ok(_vehicleMakeModelsServices.GetAll(query));
    }
}
