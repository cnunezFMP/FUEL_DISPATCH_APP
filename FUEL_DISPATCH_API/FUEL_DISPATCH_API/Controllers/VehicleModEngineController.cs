using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleModEngineController : ControllerBase
    {
        private readonly IModEngineServices _modEngineServices;
        public VehicleModEngineController(IModEngineServices modEngineServices)
        {
            _modEngineServices = modEngineServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<ModEngine>>> GetModEngines([FromQuery] GridifyQuery query)
        {
            string? companyId, branchId;
            GetUserCompanyAndBranchClass.GetUserCompanyAndBranch(out companyId, out branchId);
            return Ok(_modEngineServices.GetAll(query));
        }
    }
}
