using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeConsumptionLimitsController : ControllerBase
    {
        private readonly IEmployeeComsuptionLimits _employeeComsuptionLimitsServices;
        private readonly EmployeeComsuptionLimitsValidator _validator;

        public EmployeeConsumptionLimitsController(IEmployeeComsuptionLimits employeeComsuptionLimitsServices, EmployeeComsuptionLimitsValidator validator)
        {
            _employeeComsuptionLimitsServices = employeeComsuptionLimitsServices;
            _validator = validator;
        }

        [HttpDelete("{driverId}/DriverMethodOfComsuption/{companyId}")/*, Authorize(Roles = "Administrator")*/]
        public ActionResult<ResultPattern<DriverMethodOfComsuption>> DeleteUserCompany(int userId, int companyId)
        {
            return Ok(_employeeComsuptionLimitsServices.DeleteDriverMethod(userId, companyId));
        }

        [HttpPost/*, Authorize(Roles = "Administrator")*/]
        public ActionResult<ResultPattern<EmployeeConsumptionLimits>> SetEmployeLimitAndMethod([FromBody] EmployeeConsumptionLimits employeeConsumption)
        {
            var validationResult = _validator.Validate(employeeConsumption);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            return Created(string.Empty, new { driverId = employeeConsumption.DriverId, methodId = employeeConsumption.MethodOfComsuptionId });
        }

        [HttpPut("{userId}/DriverMethodOfComsuption/{companyId}")/*, Authorize(Roles = "Administrator")*/]
        public ActionResult<ResultPattern<DriverMethodOfComsuption>> UpdateUserCompany(int userId, int companyId)
        {
            return Ok(_employeeComsuptionLimitsServices.UpdateDriverMethod(userId, companyId));
        }
    }
}
