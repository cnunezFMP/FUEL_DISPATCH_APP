using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanySapParamsController(ICompanySapParamsServices companySapParamsServices) : ControllerBase
    {
        private readonly ICompanySapParamsServices _companySapParamsServices = companySapParamsServices;

        [HttpGet]
        public ActionResult<ResultPattern<CompanySAPParams>> GetParams([FromQuery] GridifyQuery query)
            => Ok(_companySapParamsServices.GetAll(query));

        [HttpPost]
        public ActionResult<ResultPattern<CompanySAPParams>> RegisterParams([FromBody] CompanySAPParams companySAPParams)
            => Ok(_companySapParamsServices.Post(companySAPParams));

        [HttpPut("{companyId}")]
        public ActionResult<ResultPattern<CompanySAPParams>> UpdateParams(int companyId, CompanySAPParams companySAPParams)
        {
            bool predicate(CompanySAPParams x) => x.CompanyId == companyId;
            return Ok(_companySapParamsServices.Update(predicate, companySAPParams));
        }
    }
}