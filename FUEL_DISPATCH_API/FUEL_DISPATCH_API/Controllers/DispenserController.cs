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
    public class DispenserController : ControllerBase
    {
        private readonly IValidator<Dispenser> _dispenserValidator;
        private readonly IDispenserServices _dispenserServices;
        public DispenserController(IDispenserServices dispenserServices, IValidator<Dispenser> dispenserValidator)
        {
            _dispenserServices = dispenserServices;
            _dispenserValidator = dispenserValidator;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Dispenser>>> GetDispensers([FromQuery] GridifyQuery query)
        {
            return Ok(_dispenserServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Dispenser>> GetDispenser(int id)
        {
            return Ok(_dispenserServices.Get(x => x.Id == id));
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Dispenser>> PostDispenser([FromBody] Dispenser dispenser)
        {
            var validationResult = _dispenserValidator.Validate(dispenser);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            dispenser.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            dispenser.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetDispenser), new { id = dispenser.Id }, _dispenserServices.Post(dispenser));
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Dispenser>> UpdateDispenser(int id, [FromBody] Dispenser dispenser)
        {
            dispenser.UpdatedAt = DateTime.Now;
            dispenser.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_dispenserServices.Update(x => x.Id == id, dispenser));
        }
    }
}
