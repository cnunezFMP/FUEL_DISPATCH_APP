using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
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
        private readonly IValidator<EmployeeConsumptionLimits> _validator;

        public EmployeeConsumptionLimitsController(IEmployeeComsuptionLimitsServices employeeComsuptionLimitsServices, IValidator<EmployeeConsumptionLimits> validator)
        {
            _employeeComsuptionLimitsServices = employeeComsuptionLimitsServices;
            _validator = validator;
        }

        [HttpDelete("{driverId}/DriverMethodOfComsuption/{companyId}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<DriverMethodOfComsuption>> DeleteUserCompany(int userId, int companyId)
        {
            return Ok(_employeeComsuptionLimitsServices.DeleteDriverMethod(userId, companyId));
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
            var validationResult = _validator.Validate(employeeConsumption);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            return Ok(_employeeComsuptionLimitsServices.Post(employeeConsumption));
        }

        [HttpPut("{userId}/DriverMethodOfComsuption/{companyId}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<DriverMethodOfComsuption>> UpdateUserCompany(int userId, int companyId, EmployeeConsumptionLimits employeeConsumptionLimit)
        {
            return Ok(_employeeComsuptionLimitsServices.UpdateDriverMethod(userId, companyId, employeeConsumptionLimit));
        }
    }
}
