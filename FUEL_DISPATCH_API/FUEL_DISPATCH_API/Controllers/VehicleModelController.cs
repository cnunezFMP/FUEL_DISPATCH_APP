using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleModelController : ControllerBase
    {
        private readonly IModelServices _vehicleModelServices;
        public VehicleModelController(IModelServices vehicleModelServices)
        {
            _vehicleModelServices = vehicleModelServices;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Make>> GetModels([FromQuery] GridifyQuery query)
            => Ok(_vehicleModelServices.GetAll(query));

        /// <summary>
        /// Posts a new vehicle model to the system.
        /// </summary>
        /// <param name="model">The model object to be added. This object should be provided in the request body.</param>
        /// <returns>
        /// An ActionResult object containing a ResultPattern of Make type.
        /// If the model is successfully added, the ResultPattern will contain the created model data.
        /// If an error occurs during the process, the ResultPattern will contain an error message.
        /// The HTTP status code for this request is set to 201 Created.
        /// </returns>
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Make>> PostModel([FromBody] Model model)
            => Created(string.Empty, _vehicleModelServices.Post(model));
    }
}
