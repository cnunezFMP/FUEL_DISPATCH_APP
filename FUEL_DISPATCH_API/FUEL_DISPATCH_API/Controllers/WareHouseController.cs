using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WareHouseController : ControllerBase
    {
        private readonly IWareHouseServices _wareHouseServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WareHouseController(IWareHouseServices wareHouseServices,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _wareHouseServices = wareHouseServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<WareHouse>>> GetWareHouses([FromQuery] GridifyQuery query)
            => Ok(_wareHouseServices.GetAll(query));
        
        [HttpGet("{id:int}"), Authorize]
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
        [HttpPost, Authorize(Roles = "CanCreate, Administrador")]
        public ActionResult<ResultPattern<WareHouse>> PostWareHouse([FromBody] WareHouse warehouse)
            => Created(string.Empty, _wareHouseServices.Post(warehouse));
        
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