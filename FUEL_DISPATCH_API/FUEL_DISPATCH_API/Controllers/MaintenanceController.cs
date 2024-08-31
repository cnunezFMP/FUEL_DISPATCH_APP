using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [HttpGet, Authorize(Roles = "AdminMantenimiento")]
        public ActionResult<ResultPattern<Paging<Maintenance>>> GetMaintenances([FromQuery] GridifyQuery query)
        {
            return Ok(_maintenanceServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "AdminMantenimiento")]
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
        [HttpPost, Authorize(Roles = "AdminMantenimiento")]
        public ActionResult<ResultPattern<Maintenance>> PostMaintenance([FromBody] Maintenance maintenance)
            => Created(string.Empty, _maintenanceServices.Post(maintenance));
        [HttpPut("{id:int}"), Authorize(Roles = "AdminMantenimiento")]
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
