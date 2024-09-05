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
using Twilio.Rest.Voice.V1;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchIslandController : ControllerBase
    {
        private readonly IValidator<BranchIsland> _branchIslandValidator;
        private readonly IBranchIslandServices _branchIslandServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchIslandController(IBranchIslandServices branchIslandServices,
                                      IValidator<BranchIsland> branchIslandValidator,
                                      IHttpContextAccessor httpContextAccessor)
        {
            _branchIslandServices = branchIslandServices;
            _branchIslandValidator = branchIslandValidator;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<BranchIsland>>> GetBranchIslands([FromQuery] GridifyQuery query)
            => Ok(_branchIslandServices.GetAll(query));
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<BranchIsland>> GetBranchIsland(int id)
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

            bool predicate(BranchIsland x) => x.Id == id &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId);

            return Ok(_branchIslandServices.Get(predicate));
        }
        [HttpPost, Authorize(Policy = "RegisterData, AdminRequired")]
        public ActionResult<ResultPattern<BranchIsland>> PostBranchIsland([FromBody] BranchIsland branchIsland)
            => Created(string.Empty, _branchIslandServices.Post(branchIsland));
        
        [HttpPut("{id:int}"), Authorize(Policy = "Updater, AdminRequired")]
        public ActionResult<ResultPattern<User>> UpdateBranchIsland(int id, [FromBody] BranchIsland branchIsland)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(BranchIsland x) => x.Id == id &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId);

            return Ok(_branchIslandServices.Update(predicate,
                branchIsland));
        }
    }
}
