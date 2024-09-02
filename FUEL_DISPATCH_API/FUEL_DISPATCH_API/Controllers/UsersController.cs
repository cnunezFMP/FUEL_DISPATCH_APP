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
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _usersServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(IUserServices usersServices,
                               IHttpContextAccessor httpContextAccessor)
        {
            _usersServices = usersServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<User>>> GetUsers([FromQuery] GridifyQuery query)
        {
            return Ok(_usersServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<User>> GetUser(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(User x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);

            return Ok(_usersServices.Get(predicate));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<User>> UpdateUser(int id, [FromBody] User user)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(User x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);
            return Ok(_usersServices.Update(predicate, user));
        }
        [HttpDelete("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<User>> DeleteUser(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(User x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);

            return Ok(_usersServices.Delete(predicate));
        }



    }
}
