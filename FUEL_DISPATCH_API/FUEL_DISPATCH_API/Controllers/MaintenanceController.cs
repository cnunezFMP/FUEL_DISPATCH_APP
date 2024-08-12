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
        public MaintenanceController(IMaintenanceServices maintenanceServices)
        {
            _maintenanceServices = maintenanceServices;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Maintenance>>> GetMaintenances([FromQuery] GridifyQuery query)
        {
            return Ok(_maintenanceServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Maintenance>> GetMaintenance(int id)
        {
            return Ok(_maintenanceServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Maintenance>> PostMaintenance([FromBody] Maintenance maintenance)
        {
            _maintenanceServices.SetCurrentOdometerByVehicle(maintenance);
            _maintenanceServices.SetNextMaintenanceDate(maintenance);
            _maintenanceServices.SetNextMaintenanceOdometer(maintenance);
            maintenance.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            maintenance.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Created(string.Empty, _maintenanceServices.Post(maintenance));
        }
        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Maintenance>> UpdateMaintenance(int id, [FromBody] Maintenance maintenance)
        {
            maintenance.UpdatedAt = DateTime.Now;
            maintenance.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_maintenanceServices.Update(x => x.Id == id, maintenance));
        }
    }
}
