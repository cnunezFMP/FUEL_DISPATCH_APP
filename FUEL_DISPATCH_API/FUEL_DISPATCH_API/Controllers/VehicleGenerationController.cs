using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleGenerationController : ControllerBase
    {
        private readonly IGenerationServices _vehicleGenerationServices;
        public VehicleGenerationController(IGenerationServices vehicleGenerationServices)
        {
            _vehicleGenerationServices = vehicleGenerationServices;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult GetGenerations([FromQuery] GridifyQuery query)
            => Ok(_vehicleGenerationServices.GetAll(query));
    }
}
