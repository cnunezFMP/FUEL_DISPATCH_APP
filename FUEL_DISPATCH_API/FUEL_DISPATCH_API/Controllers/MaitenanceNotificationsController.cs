using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaitenanceNotificationsController(IMaintenanceNotificacionServices maintenanceNotificacionServices) : ControllerBase
    {
        private readonly IMaintenanceNotificacionServices _maintenanceNotificacionServices = maintenanceNotificacionServices;
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<MaitenanceNotification>>> GetMaitenanceNotifications([FromQuery] GridifyQuery query)
            => Ok(_maintenanceNotificacionServices.GetAll(query));
    }
}
