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
        private readonly IValidator<Zone> _validator;
        public ZoneController(IZoneServices zoneServices, IValidator<Zone> validator)
        {
            _zoneServices = zoneServices;
            _validator = validator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Zone>>> GetZones([FromQuery] GridifyQuery query)
        {
            return Ok(_zoneServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Zone>> GetZone(int id)
        {
            return Ok(_zoneServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Zone>> PostZone([FromBody] Zone zone)
        {
            // DONE: Hacer validador de Zonas.
            var validationResult = _validator.Validate(zone);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            zone.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            zone.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetZone), new { id = zone.Id }, _zoneServices.Post(zone));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Zone>> UpdateZone(int id, [FromBody] Zone zone)
        {
            zone.UpdatedAt = DateTime.Now;
            zone.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_zoneServices.Update(x => x.Id == id, zone));
        }
    }
}
