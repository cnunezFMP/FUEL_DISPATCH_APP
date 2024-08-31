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
    public class WareHouseMovementController : ControllerBase
    {
        private readonly IWareHouseMovementServices _wareHouseMovementServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<WareHouseMovement> _wareHouseMovementValidator;
        public WareHouseMovementController(IWareHouseMovementServices wareHouseMovementServices, IValidator<WareHouseMovement> wareHouseMovementValidator, IHttpContextAccessor httpContextAccessor)
        {
            _wareHouseMovementServices = wareHouseMovementServices;
            _wareHouseMovementValidator = wareHouseMovementValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Authorize(Roles = "Despachador, Administrador")]
        public ActionResult<ResultPattern<Paging<WareHouseMovement>>> GetMovements([FromQuery] GridifyQuery query)
        {
            return Ok(_wareHouseMovementServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize(Roles = "Despachador, Administrador")]
        public ActionResult<ResultPattern<WareHouseMovement>> GetMovement(int id)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(WareHouseMovement x) => x.Id == id &&
                                           x.CompanyId == int.Parse(companyId) &&
                                           x.BranchOfficeId == int.Parse(branchId);
            return Ok(_wareHouseMovementServices.Get(predicate));
        }

        [HttpPost, Authorize(Roles = "Despachador")]
        public ActionResult<ResultPattern<WareHouseMovement>> PostMovement([FromBody] WareHouseMovement wareHouseMovement)
        {
            //var validationResult = _wareHouseMovementValidator.Validate(wareHouseMovement);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}

            return Created(string.Empty, _wareHouseMovementServices.Post(wareHouseMovement));
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Despachador")]
        public ActionResult<ResultPattern<WareHouseMovement>> UpdateMovement(int id,
            [FromBody] WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();

            branchId = _httpContextAccessor
                .HttpContext?
                .Items["BranchOfficeId"]?
                .ToString();

            bool predicate(WareHouseMovement x) => x.Id == id &&
                                           x.CompanyId == int.Parse(companyId) &&
                                           x.BranchOfficeId == int.Parse(branchId);


            return Ok(_wareHouseMovementServices
                .Update(predicate, wareHouseMovement));
        }
    }
}
