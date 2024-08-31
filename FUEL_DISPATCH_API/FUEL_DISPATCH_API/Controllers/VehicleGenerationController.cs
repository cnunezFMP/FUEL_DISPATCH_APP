using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
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

        [HttpGet, Authorize]
        public ActionResult GetGenerations([FromQuery] GridifyQuery query)
            => Ok(_vehicleGenerationServices.GetAll(query));

        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Make>> PostGen([FromBody] Generation generation)
            => Created(string.Empty, _vehicleGenerationServices.Post(generation));
    }
}
