using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersBranchOfficesController : ControllerBase
    {
        private readonly IUsersBranchOfficesServices _usersBranchOfficesServices;
        public UsersBranchOfficesController(IUsersBranchOfficesServices usersBranchOfficesServices)
        {
            _usersBranchOfficesServices = usersBranchOfficesServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<UsersBranchOffices>>> GetUsersBranchOfficess([FromQuery] GridifyQuery query)
        {
            return Ok(_usersBranchOfficesServices.GetAll(query));
        }
        [HttpDelete("{userId:int}, {branchOfficeId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersBranchOffices>> DeleteUserBranchOffice(int userId, int branchOfficeId)
        {
            Func<UsersBranchOffices, bool> predicate = x => x.UserId == userId && x.BranchOfficeId == branchOfficeId;
            return Ok(_usersBranchOfficesServices.Delete(predicate));
        }

        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersBranchOffices>> SetUsersBranchOffices([FromBody] UsersBranchOffices usersBranchOffice)
        {
            usersBranchOffice.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            usersBranchOffice.CreatedAt = DateTime.Now;
            return Ok(_usersBranchOfficesServices.Post(usersBranchOffice));
        }

        [HttpPut("{userId:int}/BranchOffice/{branchOfficeId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersBranchOffices>> UpdateUserCompany(int userId, int branchOfficeId, UsersBranchOffices usersBranchOffices)
        {
            Func<UsersBranchOffices, bool> predicate = x => x.UserId == userId && x.BranchOfficeId == branchOfficeId;
            usersBranchOffices.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            usersBranchOffices.UpdatedAt = DateTime.Now;
            return Ok(_usersBranchOfficesServices.Update(predicate, usersBranchOffices));
        }
    }
}
