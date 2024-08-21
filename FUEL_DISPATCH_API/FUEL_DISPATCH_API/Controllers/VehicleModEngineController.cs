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
    public class VehicleModEngineController : ControllerBase
    {
        private readonly IModEngineServices _engineServices;
        public VehicleModEngineController(IModEngineServices modEngineServices)
        {
            _engineServices = modEngineServices;
        }

        [HttpGet]
        public ActionResult<ResultPattern<ModEngine>> GetEngines([FromQuery] GridifyQuery query)
            => Ok(_engineServices.GetAll(query));

        [HttpPost, Authorize]
        public ActionResult<ResultPattern<ModEngine>> PostEngine([FromBody] ModEngine engine)
            => Created(string.Empty, _engineServices.Post(engine));
    }
}
