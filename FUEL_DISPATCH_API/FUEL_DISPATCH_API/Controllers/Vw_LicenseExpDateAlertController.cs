using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Vw_LicenseExpDateAlertController : ControllerBase
    {
        private readonly  ILicenseExpDateAlertServices _licenseExpDateAlertServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Vw_LicenseExpDateAlertController(ILicenseExpDateAlertServices licenseExpDateAlertServices, IHttpContextAccessor httpContextAccessor)
        {
            _licenseExpDateAlertServices = licenseExpDateAlertServices;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<ArticleDataMaster>>> GetDriverWithExpLicence([FromQuery] GridifyQuery query)
            => Ok(_licenseExpDateAlertServices.GetAll(query));
       
    }
}
