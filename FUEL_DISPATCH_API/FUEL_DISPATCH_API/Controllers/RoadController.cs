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
    public class RoadController : ControllerBase
    {
        private readonly IValidator<Road> _roadValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRoadServices _roadServices;

        public RoadController(IRoadServices roadServices,
                              IValidator<Road> roadValidator,
                              IHttpContextAccessor httpContextAccessor)
        {
            _roadServices = roadServices;
            _roadValidator = roadValidator;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Road>>> GetRoads([FromQuery] GridifyQuery query)
            => Ok(_roadServices.GetAll(query));

        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Road>> GetRoad(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(Road x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);

            return Ok(_roadServices.Get(predicate));
        }

        [HttpPost, Authorize(Roles  = "CanCreate, Administrador")]
        public ActionResult<ResultPattern<Road>> PostRoad([FromBody] Road road)
            => Created(string.Empty, _roadServices.Post(road));

        [HttpPut("{id:int}"), Authorize(Roles = "CanUpdateData, AdminRequired")]
        public ActionResult<ResultPattern<Road>> UpdateRoad(int id, [FromBody] Road road)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(Road x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);

            return Ok(_roadServices.Update(predicate, road));
        }
    }
}
