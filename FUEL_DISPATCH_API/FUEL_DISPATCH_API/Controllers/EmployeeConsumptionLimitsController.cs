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

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeConsumptionLimitsController : ControllerBase
    {
        private readonly IEmployeeComsuptionLimitsServices _employeeComsuptionLimitsServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<EmployeeConsumptionLimits> _validator;

        public EmployeeConsumptionLimitsController(IEmployeeComsuptionLimitsServices employeeComsuptionLimitsServices, IValidator<EmployeeConsumptionLimits> validator, IHttpContextAccessor httpContextAccessor)
        {
            _employeeComsuptionLimitsServices = employeeComsuptionLimitsServices;
            _httpContextAccessor = new HttpContextAccessor();
            _validator = validator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<EmployeeConsumptionLimits>>> GetEmployeeComsuptionMethods([FromQuery] GridifyQuery query)
        {
            return Ok(_employeeComsuptionLimitsServices.GetAll(query));
        }
        [HttpDelete("{driverId:int}, {methodId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<DriverMethodOfComsuption>> DeleteUserCompany(int driverId, int methodId)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            Func<EmployeeConsumptionLimits, bool> predicate = x => x.DriverId == driverId && (int)x.DriverMethodOfComsuptionId == methodId;
            return Ok(_employeeComsuptionLimitsServices.Delete(predicate));
        }
        /// <summary>
        /// Este controlador se utiliza para asignar el metodo que 
        /// tendran los conductores para el consumo de combustible 
        /// se le asigna limite y la cantidad actual. 
        /// </summary>
        /// <remarks>
        /// {
        ///   "driverId": 1,
        ///   "methodOfComsuptionId": 1,
        ///   "limitAmount": 5000,
        ///   "currentAmount": 5000
        /// }
        /// </remarks>
        /// <param name="employeeConsumption"></param>
        /// <returns></returns>
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<EmployeeConsumptionLimits>> SetEmployeLimitAndMethod([FromBody] EmployeeConsumptionLimits employeeConsumption)
        {
            var validationResult = _validator.Validate(employeeConsumption, options => options.IncludeRuleSets("InPost"));
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            employeeConsumption.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            employeeConsumption.CreatedAt = DateTime.Now;
            return Ok(_employeeComsuptionLimitsServices.Post(employeeConsumption));
        }

        [HttpPut("{driverId:int}, {methodId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<EmployeeConsumptionLimits>> UpdateUserMethod(int driverId, int methodId, EmployeeConsumptionLimits employeeConsumptionLimit)
        {
            // TODO: Ver si necesito validar la compañia y la sucursal. Y ver como hacerlo.
            Func<EmployeeConsumptionLimits, bool> predicate = x => x.DriverId == driverId && x.DriverMethodOfComsuptionId == methodId;
            employeeConsumptionLimit.UpdatedAt = DateTime.Now;
            employeeConsumptionLimit.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Created(string.Empty, _employeeComsuptionLimitsServices.Update(predicate, employeeConsumptionLimit));
        }
    }
}
