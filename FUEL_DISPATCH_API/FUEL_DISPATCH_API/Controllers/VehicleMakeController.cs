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
    public class VehicleMakeController : ControllerBase
    {
        private readonly IMakeServices _makeServices;
        public VehicleMakeController(IMakeServices makeServices)
        {
            _makeServices = makeServices;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Make>> GetMakes([FromQuery] GridifyQuery query)
            => Ok(_makeServices.GetAll(query));

        // TODO: Hacer controlador para recuperar: Las marcas, modelos, generaciones y modification engines por Id de marca. 

    }
}
