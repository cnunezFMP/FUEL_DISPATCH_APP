using FluentValidation;
using FluentValidation.Results;
using FUEL_DISPATCH_API.DataAccess.Models;
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
    public class WareHouseMovementController : ControllerBase
    {
        private readonly IWareHouseMovementServices _wareHouseMovementServices;
        private readonly IValidator<WareHouseMovement> _wareHouseMovementValidator;
        public WareHouseMovementController(IWareHouseMovementServices wareHouseMovementServices, IValidator<WareHouseMovement> wareHouseMovementValidator)
        {
            _wareHouseMovementServices = wareHouseMovementServices;
            _wareHouseMovementValidator = wareHouseMovementValidator;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<WareHouseMovement>>> GetMovements([FromQuery] GridifyQuery query)
        {
            return Ok(_wareHouseMovementServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<WareHouseMovement>> GetMovement(int id)
        {
            return Ok(_wareHouseMovementServices.Get(x => x.Id == id));
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<WareHouseMovement>> PostMovement([FromBody] WareHouseMovement wareHouseMovement)
        {
            var validationResult = _wareHouseMovementValidator.Validate(wareHouseMovement);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            wareHouseMovement.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            wareHouseMovement.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetMovement), new { id = wareHouseMovement.Id }, _wareHouseMovementServices.Post(wareHouseMovement));
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> UpdateMovement(int id, [FromBody] WareHouseMovement wareHouseMovement)
        {
            wareHouseMovement.UpdatedAt = DateTime.Now;
            wareHouseMovement.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_wareHouseMovementServices.Update(x => x.Id == id, wareHouseMovement));
        }
    }
}
