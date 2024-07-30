using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserServices _usersServices;
        public UsersController(IUserServices usersServices)
        {
            _usersServices = usersServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<User>>> GetUsers([FromQuery] GridifyQuery query)
        {
            string? companyId, branchId;
            GetUserCompanyAndBranchClass.GetUserCompanyAndBranch(out companyId, out branchId);
            return Ok(_usersServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> GetUser(int id)
        {
            return Ok(_usersServices.Get(x => x.Id == id));
        }
        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> UpdateUser(int id, [FromBody] User user)
        {
            return Ok(_usersServices.Update(x => x.Id == id, user));
        }
        [HttpDelete("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> DeleteUser(int id)
        {
            return Ok(_usersServices.Delete(x => x.Id == id));
        }



    }
}
