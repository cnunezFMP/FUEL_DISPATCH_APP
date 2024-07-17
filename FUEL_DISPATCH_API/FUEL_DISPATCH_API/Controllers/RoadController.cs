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
    public class RoadController : ControllerBase
    {
        private readonly IValidator<Road> _roadValidator;
        private readonly IRoadServices _roadServices;
        public RoadController(IRoadServices roadServices, IValidator<Road> roadValidator)
        {
            _roadServices = roadServices;
            _roadValidator = roadValidator;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<Road>>> GetRoads([FromQuery] GridifyQuery query)
        {
            return Ok(_roadServices.GetAll(query));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Road>> GetRoad(int id)
        {
            return Ok(_roadServices.Get(x => x.Id == id));
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Road>> PostRoad([FromBody] Road road)
        {
            var validationResult = _roadValidator.Validate(road);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            road.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            road.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetRoad), new { id = road.Id }, _roadServices.Post(road));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<Road>> UpdateRoad(int id, [FromBody] Road road)
        {
            road.UpdatedAt = DateTime.Now;
            road.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_roadServices.Update(x => x.Id == id, road));
        }
    }
}
