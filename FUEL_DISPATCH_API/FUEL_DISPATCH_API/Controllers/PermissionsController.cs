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
    public class PermissionsController : ControllerBase
    {
        private readonly IPermissionsServices permissionsServices;
        public PermissionsController(IPermissionsServices permissionsServices)
        {
            this.permissionsServices = permissionsServices;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Permission>>> GetPermissions([FromQuery]GridifyQuery query)
            => Ok(permissionsServices.GetAll(query));
    }
}
