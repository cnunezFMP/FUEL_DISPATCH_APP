using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesGenerationController : ControllerBase
    {
        private readonly IGenerationServices _generationServices;
        public VehiclesGenerationController(IGenerationServices generationServices)
        {
            _generationServices = generationServices;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Generation>>> GetVehicleGenerations([FromQuery] GridifyQuery query)
        {
            string? companyId, branchId;
            GetUserCompanyAndBranchClass.GetUserCompanyAndBranch(out companyId, out branchId);
            return Ok(_generationServices.GetAll(query));
        }
    }
}
