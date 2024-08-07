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
        private readonly IValidator<UsersBranchOffices> _validator;
        public UsersBranchOfficesController(IUsersBranchOfficesServices usersBranchOfficesServices, IValidator<UsersBranchOffices> validator)
        {
            _usersBranchOfficesServices = usersBranchOfficesServices;
            _validator = validator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<UsersBranchOffices>>> GetUsersBranchOfficess([FromQuery] GridifyQuery query)
        {
            return Ok(_usersBranchOfficesServices.GetAll(query));
        }
        [HttpDelete("{userId:int}/BranchOffice/{branchOfficeId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersBranchOffices>> DeleteUserBranchOffice(int userId, int branchOfficeId)
        {
            Func<UsersBranchOffices, bool> predicate = x => x.UserId == userId && x.BranchOfficeId == branchOfficeId;
            return Ok(_usersBranchOfficesServices.Delete(predicate));
        }

        // DONE: Corregir exception. (Cannot insert duplicate key in object 'dbo.UsersBranchOffices'. The duplicate key value is (1, 1)). Solution: Quite la llave primaria compuesta de la tabla UsersBranchOffices.
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersBranchOffices>> SetUsersBranchOffices([FromBody] UsersBranchOffices usersBranchOffice)
        {
            //var validationResult = _validator.Validate(usersBranchOffice);

            //if (!validationResult.IsValid)
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));

            usersBranchOffice.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            usersBranchOffice.CreatedAt = DateTime.Now;
            return Created(string.Empty, _usersBranchOfficesServices.Post(usersBranchOffice));
        }
        // DONE: Luego de resolver los problemas aqui, aplicarlo en los demas servicios. 
        [HttpPut("{userId:int}/BranchOffice/{branchOfficeId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersBranchOffices>> UpdateUserCompany(int userId, int branchOfficeId, UsersBranchOffices usersBranchOffices)
        {
            var validationResult = _validator.Validate(usersBranchOffices);

            if (!validationResult.IsValid)
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            Func<UsersBranchOffices, bool> predicate = x => x.UserId == userId && x.BranchOfficeId == branchOfficeId;
            usersBranchOffices.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            usersBranchOffices.UpdatedAt = DateTime.Now;
            return Ok(_usersBranchOfficesServices.UpdateUserBranchOffice(predicate, usersBranchOffices));
        }
    }
}
