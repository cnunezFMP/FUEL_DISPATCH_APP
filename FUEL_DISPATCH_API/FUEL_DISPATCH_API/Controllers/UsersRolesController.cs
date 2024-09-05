using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.Design;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize(Roles = "CanManageUsers, Administrador")]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUsersRolesServices _userRolesServices;
        private readonly IHttpContextAccessor _httpContextAccessor;    
        public UsersRolesController(IUsersRolesServices userRolesServices,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRolesServices = userRolesServices;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPut("{userId:int}/Roles/{rolId:int}"), Authorize]
        public ActionResult<ResultPattern<UsersRols>> UpdateUserRol(int userId, int rolId, UsersRols userRol)
        {
            bool predicate(UsersRols x) => x.UserId == userId && x.RolId == rolId;
            return Ok(_userRolesServices.UpdateUserRol(predicate, userRol));
        }
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<UsersRols>> PostUserRol([FromBody] UsersRols userRol)
            => Created(string.Empty, _userRolesServices.Post(userRol));

        [HttpDelete("{userId}/Roles/{rolId}"), Authorize]
        public ActionResult<ResultPattern<UsersRols>> DeleteUserRol(int userId, int rolId)
        {
            string? companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            Func<UsersRols, bool> predicate = x => x.UserId == userId &&
            x.RolId == rolId && 
            x.CompanyId == int.Parse(companyId);
            return Ok(_userRolesServices.Delete(predicate));
        }
    }
}
