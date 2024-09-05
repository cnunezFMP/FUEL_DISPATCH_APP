using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]"), ApiController, Authorize("AdminRequired")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleServices _roleServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RoleController(IRoleServices roleServices, IHttpContextAccessor httpContextAccessor)
        {
            _roleServices = roleServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Role>> GetRols([FromQuery] GridifyQuery query)
            => Ok(_roleServices.GetAll(query));

    }
}
