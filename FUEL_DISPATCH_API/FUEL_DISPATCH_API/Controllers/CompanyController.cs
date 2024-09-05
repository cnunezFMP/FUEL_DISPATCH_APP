using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly ICompaniesServices _companiesServices;
        // private readonly IValidator<Companies> _companiesValidator;

        public CompanyController(ICompaniesServices companiesServices/*, IValidator<Companies> companiesValidator*/)
        {
            _companiesServices = companiesServices;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Companies>>> GetComanies([FromQuery] GridifyQuery query)
            => Ok(_companiesServices.GetAll(query));


        /// <summary>
        /// Obtener todas las sucursales de una compañia. 
        /// </summary>
        /// <param name="companyId"></param>
        /// <returns></returns>
        [HttpGet("{companyId}/BranchOffice"), Authorize]
        public ActionResult<ResultPattern<BranchOffices>> GetCompanyBranchOfficess(int companyId)
            => Ok(_companiesServices.GetCompanyBranchOfficess(companyId));
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Companies>> GetCompany(int id)
            => Ok(_companiesServices.Get(x => x.Id == id));


        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Companies>> PostCompany([FromBody] Companies company)
            => Created(string.Empty, _companiesServices.Post(company));


        [HttpPut("{id:int}"), Authorize(Roles = "Administrador")]
        public ActionResult<ResultPattern<Companies>> UpdateCompanie(int id, [FromBody] Companies company)
            => Ok(_companiesServices.Update(x => x.Id == id, company));
        
    }
}
