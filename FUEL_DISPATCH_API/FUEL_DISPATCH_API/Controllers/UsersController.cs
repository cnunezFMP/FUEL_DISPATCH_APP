using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
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
        public UsersController(IUserServices usersServices)
        {
            _usersServices = usersServices;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<User>>> GetUsers([FromQuery] GridifyQuery query)
        {
            return Ok(_usersServices.GetAll(query));
        }
        [Authorize(Roles = "Administrator")]
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<User>> GetUser(int id)
        {
            return Ok(_usersServices.Get(x => x.Id == id));
        }
        [Authorize(Roles = "Administrator")]
        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<User>> UpdateUser(int id, [FromBody] User user)
        {
            return Ok(_usersServices.Update(x => x.Id == id, user));
        }
        [HttpDelete("{id:int}")]
        public ActionResult<ResultPattern<User>> DeleteUser(int id)
        {
            return Ok(_usersServices.Delete(x => x.Id == id));
        }
        [HttpPut("{userId}/Roles/{rolId}")]
        public ActionResult<ResultPattern<User>> UpdateUserRol(int userId, int rolId)
        {
            return Ok(_usersServices.UpdateUserRol(userId, rolId));
        }
        [HttpDelete("{userId}/Roles/{driverId}")]
        public ActionResult<ResultPattern<User>> DeleteUserRol(int userId, int driverId)
        {
            return Ok(_usersServices.DeleteUserRol(userId, driverId));
        }
    }
}
