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
    [ApiController]
    [Route("api/[controller]")]
    public class UsersRolesController : ControllerBase
    {
        private readonly IUsersRolesServices _userRolesServices;
        private readonly IHttpContextAccessor _httpContextAccessor;        // private readonly IValidator<UsersRols> _validator;
        public UsersRolesController(IUsersRolesServices userRolesServices,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRolesServices = userRolesServices;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPut("{userId:int}/Roles/{rolId:int}"), Authorize]
        public ActionResult<ResultPattern<UsersRols>> UpdateUserRol(int userId, int rolId, UsersRols userRol)
        {
            Func<UsersRols, bool> predicate = x => x.UserId == userId && x.RolId == rolId;
            //var validationResult = _validator.Validate(userRol);

            //if (!validationResult.IsValid)
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            return Ok(_userRolesServices.UpdateUserRol(predicate, userRol));
        }
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<UsersRols>> PostUserRol([FromBody] UsersRols userRol)
            => Created(string.Empty, _userRolesServices.Post(userRol));

        [HttpDelete("{userId}/Roles/{rolId}"), Authorize(Roles = "Administrador")]
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
