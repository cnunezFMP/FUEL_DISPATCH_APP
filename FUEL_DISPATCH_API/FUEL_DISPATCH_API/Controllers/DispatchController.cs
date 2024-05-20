using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FMP_MATEINANCEA_API.Utils;
using Microsoft.AspNetCore.Mvc;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DispatchController : ControllerBase
    {
        private readonly IDispatchServices _dispatchServices;
        public DispatchController(FUEL_DISPATCH_DBContext dBContext, IDispatchServices dispatchServices)
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
        public async Task<ActionResult<List<Dispatch>>> GetDispatches([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, int page = 1, int pageSize = 10)
        {
            try
            {
                var dispatches = await _dispatchServices.GetDispatches(startDate, endDate, page, pageSize);
                return Ok(new Responses
                {
                    Data = dispatches,
                    Message = AppConstants.DATA_OBTAINED_MESSAGE,
                    Success = true,
                    Code = StatusCodes.Status200OK,
                    PageInformation = PageInfo.PageI
                    (
                        dispatches.Count,
                        page,
                        pageSize
                    )
                });
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
        public ActionResult<Dispatch>GetDispatch(int id)
        {
            try
            {
                var dispatch = _dispatchServices.GetDispatch(id);
                return Ok(new Responses<Dispatch>(dispatch));
            }
            catch
            {
                throw;
            }
        }
        /// <summary>
        /// Create a new dispatch.
        /// </summary>
        /// <param name="dispatch"></param>
        /// <returns>Dispatch body with data posted. </returns>
        [HttpPost]
        public ActionResult<Dispatch> PostDispatch([FromBody]Dispatch dispatch)
        {
            try
            {
                _dispatchServices.CreateDispath(dispatch);
                return Ok( new Responses 
                {
                    Code = StatusCodes.Status200OK, 
                    Success = true, 
                    Data = dispatch, 
                    ObjectResult = ServiceResults.DispatchSuccessfully(dispatch.Id) 
                });
            }
            catch
            {
                throw;
            }
        }
    }
}
