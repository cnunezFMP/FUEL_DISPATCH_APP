using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpGet("{vehicleId:int}/WareHouseMovement")]
        public ActionResult<ResultPattern<Paging<Driver>>> GetDriverWareHouseMovements(int vehicleId)
        {
            return Ok(_vehicleServices.GetVehicleDispatches(vehicleId));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Vehicle>> GetVehicle(int id)
        {
            return Ok(_vehicleServices.Get(x => x.Id == id));
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Vehicle>> PostDriver([FromBody] Vehicle vehicle)
        {
            vehicle.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            vehicle.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.Id }, _vehicleServices.Post(vehicle));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<Vehicle>> UpdateUser(int id, [FromBody] Vehicle driver)
        {
            return Ok(_vehicleServices.Update(x => x.Id == id, driver));
        }
    }
}