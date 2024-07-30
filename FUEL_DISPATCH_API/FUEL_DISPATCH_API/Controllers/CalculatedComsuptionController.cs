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
    public class CalculatedComsuptionController : ControllerBase
    {
        private readonly ICalculatedComsuptionServices _calculatedComsuptionServices;
        public CalculatedComsuptionController(ICalculatedComsuptionServices calculatedComsuptionServices)
        {
            _calculatedComsuptionServices = calculatedComsuptionServices;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<CalculatedComsuption>>> GetCalculatedComsuption([FromQuery] GridifyQuery query)
        {
            string? companyId, branchId;
            GetUserCompanyAndBranchClass.GetUserCompanyAndBranch(out companyId, out branchId);
            return Ok(_calculatedComsuptionServices.GetAll(query));
        }
    }
}
