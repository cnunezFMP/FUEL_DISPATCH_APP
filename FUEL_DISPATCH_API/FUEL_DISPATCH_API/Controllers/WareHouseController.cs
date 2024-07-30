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
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseServices _wareHouseServices;
        private readonly IValidator<WareHouse> _wareHouseValidator;

        public WareHouseController(IWareHouseServices wareHouseServices, IValidator<WareHouse> wareHouseValidator)
        {
            _wareHouseServices = wareHouseServices;
            _wareHouseValidator = wareHouseValidator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<WareHouse>>> GetWareHouses([FromQuery] GridifyQuery query)
        {
            return Ok(_wareHouseServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<WareHouse>> GetWareHouse(int id)
        {
            return Ok(_wareHouseServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<WareHouse>> PostWareHouse([FromBody] WareHouse warehouse)
        {
            var validationResult = _wareHouseValidator.Validate(warehouse);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            warehouse.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            warehouse.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreatedAtAction(nameof(GetWareHouse), new { id = warehouse.Id }, _wareHouseServices.Post(warehouse));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<WareHouse>> UpdateStore(int id, [FromBody] WareHouse warehouse)
        {
            return Ok(_wareHouseServices.Update(x => x.Id == id, warehouse));
        }
    }
}