using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly IDispatchServices _dispatchServices;
        public DispatchController(IDispatchServices dispatchServices)
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
        public ActionResult<ResultPattern<Paging<Dispatch>>> GetDispatches([FromQuery] GridifyQuery query)
        {
            var dispatches = _dispatchServices.GetAll(query);
            return Ok(dispatches);
        }

        /// <summary>
        /// Get a dispatch by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The dispatch</returns>
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Dispatch>> GetDispatch(int id)
        {
            Func<Dispatch, bool> findId = x => x.Id == id;
            var dispatch = _dispatchServices.Get(findId);
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
            _dispatchServices.Post(dispatch);
            return Ok(ResultPattern<Dispatch>.Success(dispatch, StatusCodes.Status200OK, "Despacho Creado"));
        }
    }
}