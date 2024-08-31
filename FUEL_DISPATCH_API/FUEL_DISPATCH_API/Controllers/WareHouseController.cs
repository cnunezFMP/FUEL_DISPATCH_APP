using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<WareHouse> _wareHouseValidator;

        public WareHouseController(IWareHouseServices wareHouseServices, 
                                   IValidator<WareHouse> wareHouseValidator,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _wareHouseServices = wareHouseServices;
            _wareHouseValidator = wareHouseValidator;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize(Roles = "Administrador"]
        public async Task<ActionResult<ResultPattern<Paging<WareHouse>>>> GetWareHouses([FromQuery] GridifyQuery query)
        {
            return Ok(_wareHouseServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<WareHouse>> GetWareHouse(int id)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(WareHouse x) => x.Id == id &&
                                           x.CompanyId == int.Parse(companyId) &&
                                           x.BranchOfficeId == int.Parse(branchId);
            return Ok(_wareHouseServices.Get(predicate));
        }
        [HttpPost, Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<WareHouse>> PostWareHouse([FromBody] WareHouse warehouse)
        {
            //var validationResult = _wareHouseValidator.Validate(warehouse);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            return Created(string.Empty, _wareHouseServices.Post(warehouse));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<WareHouse>> UpdateStore(int id, [FromBody] WareHouse warehouse)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(WareHouse x) => x.Id == id &&
                                           x.CompanyId == int.Parse(companyId) &&
                                           x.BranchOfficeId == int.Parse(branchId);

            return Ok(_wareHouseServices.Update(predicate, warehouse));
        }
    }
}