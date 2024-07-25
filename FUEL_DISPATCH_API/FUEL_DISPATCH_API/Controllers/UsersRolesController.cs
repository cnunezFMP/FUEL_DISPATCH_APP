using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUsersRolesServices _userRolesServices;
        public UsersRolesController(IUsersRolesServices userRolesServices)
        {
            _userRolesServices = userRolesServices;
        }

        [HttpPut("{userId}/Roles/{rolId}")]
        public ActionResult<ResultPattern<User>> UpdateUserRol(int userId, int rolId)
        {
            return Ok(_userRolesServices.UpdateUserRol(userId, rolId));
        }
        [HttpDelete("{userId}/Roles/{rolId}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> DeleteUserRol(int userId, int rolId)
        {
            return Ok(_userRolesServices.DeleteUserRol(userId, rolId));
        }
    }
}
