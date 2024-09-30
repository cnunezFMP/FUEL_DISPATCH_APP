using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController(IVw_NotificationsServices notificationsServices) : ControllerBase
    {
        private readonly IVw_NotificationsServices _notificationsServices = notificationsServices;
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Vw_Notifications>>> GetNotifications([FromQuery] GridifyQuery query)
            => Ok(_notificationsServices.GetAll(query));

    }
}
