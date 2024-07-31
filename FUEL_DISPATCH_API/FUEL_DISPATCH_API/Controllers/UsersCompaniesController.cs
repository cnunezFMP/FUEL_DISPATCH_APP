using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersCompaniesController : ControllerBase
    {
        private readonly ICompaniesUsersServices _companiesUsersServices;
        private readonly IValidator<UsersCompanies> _validator;

        public UsersCompaniesController(ICompaniesUsersServices companiesUsersServices, IValidator<UsersCompanies> validator)
        {
            _companiesUsersServices = companiesUsersServices;
            _validator = validator;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<UsersCompanies>>> GetDrivers([FromQuery] GridifyQuery query)
        {
            return Ok(_companiesUsersServices.GetAll(query));
        }
        [HttpDelete("{userId}/Company/{companyId}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> DeleteUserCompany(int userId, int companyId)
        {
            Func<UsersCompanies, bool> predicate = x => x.UserId == userId && x.CompanyId == companyId;
            return Ok(_companiesUsersServices.Delete(predicate));
        }

        /// <summary>
        /// Actualizar relaciones Usuario-Compañia.
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>
        /// <param name="userId">Id del usuario. </param>
        /// <param name="companyId">Id de la compañia. </param>
        /// <param name="usersCompany">El cuerpo de la relacion. </param>
        /// <response code="200">Si se actualiza la entidad correctamente. </response>
        /// <response code="400">Si falla una validacion. </response>
        /// <response code="404">Si no se encuentra la endidad con los Id que se proveen. </response>
        /// <returns></returns>
        [HttpPut("{userId}/Company/{companyId}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<User>> UpdateUserCompany(int userId, int companyId, UsersCompanies usersCompany)
        {
            Func<UsersCompanies, bool> predicate = x => x.UserId == userId && x.CompanyId == companyId;
            var validationResult = _validator.Validate(usersCompany);

            if (!validationResult.IsValid)
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            usersCompany.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            usersCompany.UpdatedAt = DateTime.Now;
            return Ok(_companiesUsersServices.Update(predicate, usersCompany));
        }
        /// <summary>
        /// Agregar una nueva relacion Usuario-Compañia. 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="usersCompany"></param>
        /// <response code="201">Si se actualiza la entidad correctamente. </response>
        /// <response code="400">Si falla una validacion. </response>
        /// <returns></returns>
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<UsersCompanies>> SetUsersCompany([FromBody] UsersCompanies usersCompany)
        {
            var validationResult = _validator.Validate(usersCompany);

            if (!validationResult.IsValid)
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));

            usersCompany.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            usersCompany.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Created(string.Empty, _companiesUsersServices.Post(usersCompany));
        }
    }
}
