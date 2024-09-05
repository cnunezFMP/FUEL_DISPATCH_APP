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


        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Role>> CreateRol([FromBody] Role role)
            => Created(string.Empty, _roleServices.Post(role));

        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Role>> UpdateRol(int id, [FromBody] Role role)
        {
            string? companyid = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();


            bool predicate(Role x) => x.Id == id &&
                x.CompanyId == int.Parse(companyid);


            return Ok(_roleServices.Update(predicate, role));
        }
    }
}
