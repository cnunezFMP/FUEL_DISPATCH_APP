using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModEngineController : ControllerBase
    {
        private readonly IModEngineServices _modEngineServices;
        public ModEngineController(IModEngineServices modEngineServices)
        {
            _modEngineServices = modEngineServices;
        }
        [HttpGet, Authorize]
        public ActionResult GetModEngines([FromQuery] GridifyQuery query)
            => Ok(_modEngineServices.GetAll(query));
    }
}
