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
        public BranchOfficeController(IValidator<BranchOffices> branchOfficeValidator, IBranchOfficeServices branchOfficeServices)
        {
            _branchOfficeValidator = branchOfficeValidator;
            _branchOfficeServices = branchOfficeServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<BranchOffices>>> GetBranchOfficess([FromQuery] GridifyQuery query)
        {
            return Ok(_branchOfficeServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<BranchOffices>> GetBranchOffice(int id)
        {
            return Ok(_branchOfficeServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<BranchOffices>> PostBranchOffice([FromBody] BranchOffices branchOffice)
        {
            //var validationResult = _branchOfficeValidator.Validate(branchOffice);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            branchOffice.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            branchOffice.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            // DONE: Fix this, currently throws exception 'InvalidOperationException: No route matches the supplied values.'
            return CreatedAtAction(nameof(GetBranchOffice),
                new
                {
                    id = branchOffice.Id
                }, _branchOfficeServices.Post(branchOffice));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<BranchOffices>> UpdateBranchOffice(int id, [FromBody] BranchOffices branchOffice)
        {
            branchOffice.UpdatedAt = DateTime.Now;
            branchOffice.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_branchOfficeServices.Update(x => x.Id == id, branchOffice));
        }
    }
}
