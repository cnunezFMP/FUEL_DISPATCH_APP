using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly IDispatchServices _dispatchServices;
        public DispatchController(FUEL_DISPATCH_DBContext dBContext, IDispatchServices dispatchServices, IGridifyServices<Dispatch> gridifyServices)
        {
            _dispatchServices = dispatchServices;
        }

        /// <summary>
        /// Get a list of dispatches.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<Dispatch>>> GetDispatches([FromQuery] GridifyQuery query)
        {
            try
            {
                var dispatches = _dispatchServices.GetAll(query);
                return Ok(dispatches.Data);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get a dispatch by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The dispatch</returns>
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Dispatch>> GetDispatch(int id)
        {
            var dispatch = _dispatchServices.GetDispatch(id);
            if (dispatch.IsSuccess is not true)
                return BadRequest(dispatch);
            return Ok(dispatch);
        }

        /// <summary>
        /// Create a new dispatch.
        /// </summary>
        /// <param name="dispatch"></param>
        /// <returns>Dispatch body with data posted.</returns>
        [HttpPost]
        public ActionResult<ResultPattern<Dispatch>> PostDispatch([FromBody] Dispatch dispatch)
        {
            var createdDispatch = _dispatchServices.CreateDispath(dispatch);

            if (createdDispatch.IsSuccess is true)
                return Ok(createdDispatch);

            return BadRequest(createdDispatch);
        }
    }
}