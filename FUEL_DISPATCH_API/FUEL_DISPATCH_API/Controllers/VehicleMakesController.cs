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
    public class VehicleMakesController : ControllerBase
    {
        private readonly IMakeServices _makeServices;
        public VehicleMakesController(IMakeServices makeServices)
        {
            _makeServices = makeServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Make>>> GetMakes([FromQuery] GridifyQuery query)
        {
            return Ok(_makeServices.GetAll(query));
        }
    }
}
