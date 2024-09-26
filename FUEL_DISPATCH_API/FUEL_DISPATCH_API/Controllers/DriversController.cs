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
    public class DriversController : ControllerBase
    {
        private readonly IValidator<Driver> _driverValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDriversServices _driverServices;
        public DriversController(IDriversServices driverServices, IValidator<Driver> driverValidator, IHttpContextAccessor httpContextAccessor)
        {
            _driverServices = driverServices;
            _driverValidator = driverValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Driver>>> GetDrivers([FromQuery] GridifyQuery query)
            => Ok(_driverServices.GetAll(query));


        [HttpGet("{driverId:int}/WareHouseMovement"), Authorize/*(Roles = "CanReadData, Administrador, CanGenerateReport")*/]
        public ActionResult<ResultPattern<Paging<Driver>>> GetDriverWareHouseMovements(int driverId)
            => Ok(_driverServices.GetDriverDispatches(driverId));

        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Driver>> GetDriver(int id)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            bool predicate(Driver x) => x.Id == id; /*&&
                                               x.CompanyId == int.Parse(companyId) &&
                                               x.BranchOfficeId == int.Parse(branchId)*/
            return Ok(_driverServices.Get(predicate));
        }

        /// <summary>
        /// Agrega un nuevo conductor
        /// </summary>
        /// <remarks>
        /// El email no es obligatorio, solo es en caso de que el conductor vaya a tener email.
        /// 
        /// Sample request: 
        /// 
        ///     {
        ///          "birthDate": "2000-01-04 00:00:00:000",
        ///          "branchOfficeId": "1",
        ///          "fullDirection": "C/ Luis Padilla #53",
        ///          "fullName": "Jhon Doe",
        ///          "identification": "001-0239485-3",
        ///          "phoneNumber": "8090909828",
        ///          "email": "e@gmail.com",
        ///          "licenceExpDate": "2024-01-04 00:00:00:000"
        ///     }
        /// </remarks>
        /// <param name="driver"></param>
        /// <response code="201">Conductor registrado</response>
        /// <response code="400">Si alguna validacion falla</response>
        /// <returns></returns>
        [HttpPost, Authorize/*(Roles = "CanCreate, Administrador")*/]
        public ActionResult<ResultPattern<Driver>> PostDriver([FromBody] Driver driver)
            => Created(string.Empty, _driverServices.Post(driver));


        [HttpPut("{id:int}"), Authorize/*(Roles = "Administrador")*/]
        public ActionResult<ResultPattern<Driver>> UpdateDriver(int id, [FromBody] Driver driver)
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

            bool predicate(Driver x) => x.Id == id /*&&
                                               x.CompanyId == int.Parse(companyId) &&
                                               x.BranchOfficeId == int.Parse(branchId)*/;
            return Ok(_driverServices.Update(predicate, driver));
        }
    }
}
