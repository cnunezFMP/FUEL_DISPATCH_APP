using FluentValidation;
using FluentValidation.Results;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly IDriversServices _driverServices;
        private readonly IValidator<Driver> _driverValidator;

        public DriversController(IDriversServices driverServices, IValidator<Driver> driverValidator)
        {
            _driverServices = driverServices;
            _driverValidator = driverValidator;
        }

        [HttpGet]
        public ActionResult<ResultPattern<Paging<Driver>>> GetDrivers([FromQuery] GridifyQuery query)
        {
            return Ok(_driverServices.GetAll(query));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Driver>> GetDriver(int id)
        {
            return Ok(_driverServices.Get(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult<ResultPattern<Driver>> PostDriver([FromBody] Driver driver)
        {
            var validationResult = _driverValidator.Validate(driver);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            driver.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            driver.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, _driverServices.Post(driver));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<User>> UpdateUser(int id, [FromBody] Driver driver)
        {
            driver.UpdatedAt = DateTime.Now;
            driver.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_driverServices.Update(x => x.Id == id, driver));
        }
    }
}
