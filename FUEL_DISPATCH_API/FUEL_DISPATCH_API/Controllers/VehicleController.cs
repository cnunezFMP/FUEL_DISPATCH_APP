using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
// DONE: Revisar este controlador. Y corregir la validacion en FluentValidation.
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehiclesServices _vehicleServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Vehicle> _vehicleValidator;
        public VehicleController(IVehiclesServices vehicleServices,
            IValidator<Vehicle> validator)
        {
            _vehicleServices = vehicleServices;
            _vehicleValidator = validator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Vehicle>>> GetVehicles([FromQuery] GridifyQuery query)
        {
            return Ok(_vehicleServices.GetAll(query));
        }
        [HttpGet("{vehicleId:int}/WareHouseMovement"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Vehicle>>> GetVehicleWareHouseMovements(int vehicleId)
        {
            return Ok(_vehicleServices.GetVehicleDispatches(vehicleId));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Vehicle>> GetVehicle(int id)
        {
            return Ok(_vehicleServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Vehicle>> PostVehicle([FromBody] Vehicle vehicle)
        {
            //var result = _vehicleValidator.Validate(vehicle);
            //if (!result.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(result));
            //}
            
            return Created(string.Empty, _vehicleServices.Post(vehicle));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Vehicle>> UpdateVehicle(int id, [FromBody] Vehicle vehicle)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(Vehicle x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId) &&
                                      x.BranchOfficeId == int.Parse(branchId);
            //var result = _vehicleValidator.Validate(vehicle);
            //if (!result.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(result));
            //}
            return Ok(_vehicleServices.Update(predicate, vehicle));
        }
    }
}