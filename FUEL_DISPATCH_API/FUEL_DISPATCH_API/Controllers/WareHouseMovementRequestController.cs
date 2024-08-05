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
    public class WareHouseMovementRequestController : ControllerBase
    {
        private readonly IRequestServices _requestServices;
        private readonly IValidator<WareHouseMovementRequest> _validator;
        public WareHouseMovementRequestController(IRequestServices requestServices, IValidator<WareHouseMovementRequest> validator)
        {
            _requestServices = requestServices;
            _validator = validator;
        }

        [HttpGet, Authorize(Roles = "Administrator, Dispatcher")]
        public ActionResult<ResultPattern<Paging<WareHouseMovementRequest>>> GetRequests([FromQuery] GridifyQuery query)
        {
            return Ok(_requestServices.GetAll(query));
        }
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<WareHouseMovementRequest>> GetRequest(int id)
        {
            return Ok(_requestServices.Get(x => x.Id == id));
        }
        // DONE: Agregar el validador aqui.
        [HttpPost]
        public ActionResult<ResultPattern<WareHouseMovementRequest>> PostRequest([FromBody] WareHouseMovementRequest request)
        {
            var validationResult = _validator.Validate(request, option => option.IncludeRuleSets("WareHouses"));
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            request.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            request.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreatedAtAction(nameof(GetRequest), new
            {
                id = request.Id
            }, _requestServices.Post(request));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<User>> UpdateRequest(int id, [FromBody] WareHouseMovementRequest request)
        {
            request.UpdatedAt = DateTime.Now;
            request.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_requestServices.Update(x => x.Id == id, request));
        }
    }
}
