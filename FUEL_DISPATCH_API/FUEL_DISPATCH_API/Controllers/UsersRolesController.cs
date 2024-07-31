using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;

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

        [HttpPut("{userId:int}/Roles/{rolId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersRols>> UpdateUserRol(int userId, int rolId, UsersRols userRol)
        {
            Func<UsersRols, bool> predicate = x => x.UserId == userId && x.RolId == rolId;
            userRol.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            userRol.UpdatedAt = DateTime.Now;
            return Ok(_userRolesServices.UpdateUserRol(predicate, userRol));
        }
        [HttpPost]
        public ActionResult<ResultPattern<UsersRols>> PostUserRol([FromBody] UsersRols userRol)
        {
            return Created(string.Empty, _userRolesServices.Post(userRol));
        }
        [HttpDelete("{userId}/Roles/{rolId}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersRols>> DeleteUserRol(int userId, int rolId)
        {
            Func<UsersRols, bool> predicate = x => x.UserId == userId && x.RolId == rolId;
            return Ok(_userRolesServices.Delete(predicate));
        }
    }
}
