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
        private readonly IMakeServices _vehicleMakeServices;
        public VehicleMakeController(IMakeServices makeServices)
        {
            _vehicleMakeServices = makeServices;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Make>> GetMakes([FromQuery] GridifyQuery query)
            => Ok(_vehicleMakeServices.GetAll(query));

        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Make>> PostMake([FromBody] Make make)
            => Created(string.Empty, _vehicleMakeServices.Post(make));

        // TODO: Hacer controlador para recuperar: Las marcas, modelos, generaciones y modification engines por Id de marca. 

    }
}
