using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehiclesServices _vehicleServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Vehicle> _vehicleValidator;
        public VehicleController(IVehiclesServices vehicleServices,
            IValidator<Vehicle> validator,
            IHttpContextAccessor _httpContextAccessor)
        {
            _vehicleServices = vehicleServices;
            _vehicleValidator = validator;
            this._httpContextAccessor = _httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Vehicle>>> GetVehicles([FromQuery] GridifyQuery query)
            => Ok(_vehicleServices.GetAll(query));

        [HttpGet("{vehicleId:int}/WareHouseMovement"), Authorize]
        public ActionResult<ResultPattern<Paging<Vehicle>>> GetVehicleWareHouseMovements(int vehicleId)
            => Ok(_vehicleServices.GetVehicleDispatches(vehicleId));
        
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Vehicle>> GetVehicle(int id)
            => Ok(_vehicleServices.Get(x => x.Id == id));

        [HttpPost, Authorize(Roles = "CanManageVehicles, Administrador")]
        public ActionResult<ResultPattern<Vehicle>> PostVehicle([FromBody] Vehicle vehicle)
            => Created(string.Empty, _vehicleServices.Post(vehicle));

        [HttpPut("{id:int}"), Authorize(Roles = "CanManageVehicles, Administrador")]
        public ActionResult<ResultPattern<Vehicle>> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();

            branchId = _httpContextAccessor
                .HttpContext?
                .Items["BranchOfficeId"]?
                .ToString();*/

            bool predicate(Vehicle x) => x.Id == id/* &&
                                      x.CompanyId == int.Parse(companyId) &&
                                      x.BranchOfficeId == int.Parse(branchId)*/;

            return Ok(_vehicleServices.Update(predicate, vehicle));
        }
    }
}