using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehiclesServices _vehicleServices;

        public VehicleController(IVehiclesServices vehicleServices)
        {
            _vehicleServices = vehicleServices;
        }

        [HttpGet]
        public ActionResult<ResultPattern<Paging<Vehicle>>> GetVehicles([FromQuery] GridifyQuery query)
        {
            return Ok(_vehicleServices.GetAll(query));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Vehicle>> GetVehicle(int id)
        {
            return Ok(_vehicleServices.Get(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult<ResultPattern<Vehicle>> PostDriver([FromBody] Vehicle vehicle)
        {
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, _vehicleServices.Post(vehicle));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<Vehicle>> UpdateUser(int id, [FromBody] Vehicle driver)
        {
            return Ok(_vehicleServices.Update(x => x.Id == id, driver));
        }
    }
}