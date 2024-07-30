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
    [Authorize(Roles = "Administrator")]
    [ApiController]
    [Route("api/[controller]")]
    public class BranchIslandController : ControllerBase
    {
        private readonly IValidator<BranchIsland> _branchIslandValidator;
        private readonly IBranchIslandServices _branchIslandServices;
        public BranchIslandController(IBranchIslandServices branchIslandServices, IValidator<BranchIsland> branchIslandValidator)
        {
            _branchIslandServices = branchIslandServices;
            _branchIslandValidator = branchIslandValidator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<BranchIsland>>> GetBranchIslands([FromQuery] GridifyQuery query)
        {
           
            return Ok(_branchIslandServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<BranchIsland>> GetBranchIsland(int id)
        {
            return Ok(_branchIslandServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<BranchIsland>> PostBranchIsland([FromBody] BranchIsland branchIsland)
        {
            var validationResult = _branchIslandValidator.Validate(branchIsland);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            branchIsland.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            branchIsland.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreatedAtAction(nameof(GetBranchIsland), new { id = branchIsland.Id }, _branchIslandServices.Post(branchIsland));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> UpdateBranchIsland(int id, [FromBody] BranchIsland branchIsland)
        {
            branchIsland.UpdatedAt = DateTime.Now;
            branchIsland.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_branchIslandServices.Update(x => x.Id == id, branchIsland));
        }
    }
}
