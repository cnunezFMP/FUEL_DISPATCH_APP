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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchOfficeController : ControllerBase
    {
        private readonly IValidator<BranchOffices> _branchOfficeValidator;
        private readonly IBranchOfficeServices _branchOfficeServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchOfficeController(IValidator<BranchOffices> branchOfficeValidator,
                                      IBranchOfficeServices branchOfficeServices,
                                      IHttpContextAccessor httpContextAccessor)
        {
            _branchOfficeValidator = branchOfficeValidator;
            _branchOfficeServices = branchOfficeServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<BranchOffices>>> GetBranchOfficess([FromQuery] GridifyQuery query)
        {
            return Ok(_branchOfficeServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<BranchOffices>> GetBranchOffice(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor
                        .HttpContext?
                        .Items["CompanyId"]?
                        .ToString();

            bool predicate(BranchOffices x) => x.Id == id &&
                                               x.CompanyId == int.Parse(companyId);
            return Ok(_branchOfficeServices.Get(predicate));
        }
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<BranchOffices>> PostBranchOffice([FromBody] BranchOffices branchOffice)
            => Created(string.Empty, _branchOfficeServices.Post(branchOffice));
            // DONE: Fix this, currently throws exception 'InvalidOperationException: No route matches the supplied values.'

        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<BranchOffices>> UpdateBranchOffice(int id, [FromBody] BranchOffices branchOffice)
        {
            string? companyId;
            companyId = _httpContextAccessor
                        .HttpContext?
                        .Items["CompanyId"]?
                        .ToString();

            bool predicate(BranchOffices x) => x.Id == id &&
                                               x.CompanyId == int.Parse(companyId);
            return Ok(_branchOfficeServices.Update(predicate, branchOffice));
        }
    }
}
