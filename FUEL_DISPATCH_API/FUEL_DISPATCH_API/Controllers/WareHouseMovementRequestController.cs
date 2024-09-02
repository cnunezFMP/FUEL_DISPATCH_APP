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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<WareHouseMovementRequest> _validator;
        public WareHouseMovementRequestController(IRequestServices requestServices,
            IValidator<WareHouseMovementRequest> validator,
            IHttpContextAccessor _httpContextAccessor)
        {
            _requestServices = requestServices;
            _validator = validator;
            this._httpContextAccessor = _httpContextAccessor;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<WareHouseMovementRequest>>> GetRequests([FromQuery] GridifyQuery query)
        {
            return Ok(_requestServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<WareHouseMovementRequest>> GetRequest(int id)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(WareHouseMovementRequest x) => x.Id == id &&
                                           x.CompanyId == int.Parse(companyId) &&
                                           x.BranchOfficeId == int.Parse(branchId);

            return Ok(_requestServices.Get(predicate));
        }
        // DONE: Agregar el validador aqui.
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<WareHouseMovementRequest>> PostRequest([FromBody] WareHouseMovementRequest request)
        {
            var validationResult = _validator.Validate(request, option => option.IncludeRuleSets("WareHouses"));
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            return Created(string.Empty, _requestServices.Post(request));
        }

        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<WareHouseMovementRequest>> UpdateRequest(int id, [FromBody] WareHouseMovementRequest request)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(WareHouseMovementRequest x) => x.Id == id &&
                                           x.CompanyId == int.Parse(companyId) &&
                                           x.BranchOfficeId == int.Parse(branchId);

            return Ok(_requestServices.Update(predicate, request));
        }
    }
}
