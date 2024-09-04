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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<UsersBranchOffices> _validator;
        public UsersBranchOfficesController(IUsersBranchOfficesServices usersBranchOfficesServices, IValidator<UsersBranchOffices> validator, 
          IHttpContextAccessor httpContextAccessor)
        {
            _usersBranchOfficesServices = usersBranchOfficesServices;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<UsersBranchOffices>>> GetUsersBranchOfficess([FromQuery] GridifyQuery query)
        {
            return Ok(_usersBranchOfficesServices.GetAll(query));
        }
        [HttpDelete("{userId:int}/BranchOffice/{branchOfficeId:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<UsersBranchOffices>> DeleteUserBranchOffice(int userId)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(UsersBranchOffices x) => x.BranchOfficeId == int.Parse(branchId) &&
                                                    x.UserId == userId &&
                                                    x.CompanyId == int.Parse(companyId);


            return Ok(_usersBranchOfficesServices.Delete(predicate));
        }

        // DONE: Corregir exception. (Cannot insert duplicate key in object 'dbo.UsersBranchOffices'. The duplicate key Value is (1, 1)). Solution: Quite la llave primaria compuesta de la tabla UsersBranchOffices.
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<UsersBranchOffices>> SetUsersBranchOffices([FromBody] UsersBranchOffices usersBranchOffice)
            => Created(string.Empty, _usersBranchOfficesServices.Post(usersBranchOffice));
        
        // DONE: Luego de resolver los problemas aqui, aplicarlo en los demas servicios. 
        [HttpPut("{userId:int}/BranchOffice/{branchOfficeId:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<UsersBranchOffices>> UpdateUserCompany(int userId,
            UsersBranchOffices usersBranchOffices)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(UsersBranchOffices x) => x.BranchOfficeId == int.Parse(branchId) &&
                                                    x.UserId == userId &&
                                                    x.CompanyId == int.Parse(companyId);

            return Ok(_usersBranchOfficesServices.UpdateUserBranchOffice(predicate, usersBranchOffices));
        }
    }
}
