using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverMethodOfComsuptionController : ControllerBase
    {
        private readonly IDriverMethodOfComsuptionServices _driverMethodOfComsuptionServices;

        public DriverMethodOfComsuptionController(IDriverMethodOfComsuptionServices driverMethodOfComsuptionServices)
        {
            _driverMethodOfComsuptionServices = driverMethodOfComsuptionServices;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<ArticleDataMaster>>> GetArticles([FromQuery] GridifyQuery query)
        {
            return Ok(_driverMethodOfComsuptionServices.GetAll(query));
        }
    }
}
