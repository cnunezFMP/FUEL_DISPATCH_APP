using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
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
    public class ZoneController : ControllerBase
    {
        private readonly IZoneServices _zoneServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<Zone> _validator;
        public ZoneController(IZoneServices zoneServices,
                              IValidator<Zone> validator,
                              IHttpContextAccessor httpContextAccessor)
        {
            _zoneServices = zoneServices;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Zone>>> GetZones([FromQuery] GridifyQuery query)
        {
            return Ok(_zoneServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Zone>> GetZone(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(Zone x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);

            return Ok(_zoneServices.Get(predicate));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Zone>> PostZone([FromBody] Zone zone)
        {
            // DONE: Hacer validador de Zonas.
            //var validationResult = _validator.Validate(zone);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            return Created(string.Empty, _zoneServices.Post(zone));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Zone>> UpdateZone(int id, [FromBody] Zone zone)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(Zone x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);


            return Ok(_zoneServices.Update(predicate, zone));
        }
    }
}
