using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize(Policy = "MaitenanceManagement")]
    public class MaintenanceController : ControllerBase
    {
        private readonly IMaintenanceServices _maintenanceServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MaintenanceController(IMaintenanceServices maintenanceServices,
                                     IHttpContextAccessor httpContextAccessor)
        {
            _maintenanceServices = maintenanceServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Maintenance>>> GetMaintenances([FromQuery] GridifyQuery query)
            => Ok(_maintenanceServices.GetAll(query));
        
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Maintenance>> GetMaintenance(int id)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(Maintenance x) => x.Id == id &&
                                               x.CompanyId == int.Parse(companyId) &&
                                               x.BranchOfficeId == int.Parse(branchId);

            return Ok(_maintenanceServices.Get(predicate));
        }
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Maintenance>> PostMaintenance([FromBody] Maintenance maintenance)
            => Created(string.Empty, _maintenanceServices.Post(maintenance));

        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Maintenance>> UpdateMaintenance(int id, [FromBody] Maintenance maintenance)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(Maintenance x) => x.Id == id &&
                                               x.CompanyId == int.Parse(companyId) &&
                                               x.BranchOfficeId == int.Parse(branchId);

            return Ok(_maintenanceServices.Update(predicate, maintenance));
        }
    }
}
