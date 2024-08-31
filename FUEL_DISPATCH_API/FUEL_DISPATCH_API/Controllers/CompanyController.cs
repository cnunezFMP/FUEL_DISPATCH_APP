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
    public class CompanyController : ControllerBase
    {
        private readonly ICompaniesServices _companiesServices;
        private readonly IValidator<Companies> _companiesValidator;

        public CompanyController(ICompaniesServices companiesServices, IValidator<Companies> companiesValidator)
        {
            _companiesServices = companiesServices;
            _companiesValidator = companiesValidator;
        }

        [HttpGet, Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<Paging<Companies>>> GetComanies([FromQuery] GridifyQuery query)
        {
            return Ok(_companiesServices.GetAll(query));
        }

        /// <summary>
        /// Obtener todas las sucursales de una compañia. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("{companyId}/BranchOffice"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<BranchOffices>> GetCompanyBranchOfficess(int companyId)
        {
            return Ok(_companiesServices.GetCompanyBranchOfficess(companyId));
        }
        [HttpGet("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<Companies>> GetCompany(int id)
        {
            return Ok(_companiesServices.Get(x => x.Id == id));
        }

        //[HttpGet("{companyRnc}"), Authorize(Roles = "Administrator")]
        //public ActionResult<ResultPattern<Companies>> GetCompany(string companyRnc)
        //{
        //    return Ok(_companiesServices.GetCompanyByRnc(companyRnc));
        //}

        [HttpPost, Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<Companies>> PostCompany([FromBody] Companies company)
        {
            var validationResult = _companiesValidator.Validate(company);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            return Created(string.Empty, _companiesServices.Post(company));
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<Companies>> UpdateCompanie(int id, [FromBody] Companies company)
        {
            return Ok(_companiesServices.Update(x => x.Id == id, company));
        }
    }
}
