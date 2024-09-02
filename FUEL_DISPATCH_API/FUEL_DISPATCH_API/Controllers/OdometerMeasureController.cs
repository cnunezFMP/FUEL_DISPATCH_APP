using FluentValidation;
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
    public class OdometerMeasureController : ControllerBase
    {
        private readonly IOdometerMeasureServices _odometerMeasureServices;
        public OdometerMeasureController(IOdometerMeasureServices odometerMeasureServices)
        {
            _odometerMeasureServices = odometerMeasureServices;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<BranchIsland>>> GetOdometerMeasures([FromQuery] GridifyQuery query)
        {
            return Ok(_odometerMeasureServices.GetAll(query));
        }
    }
}
