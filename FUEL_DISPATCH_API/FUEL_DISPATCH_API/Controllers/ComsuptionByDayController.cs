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
    public class ComsuptionByDayController : Controller
    {
        private readonly IComsuptionByDayServices _comsuptionByDayServices;
        public ComsuptionByDayController(IComsuptionByDayServices comsuptionByDayServices)
        {
            _comsuptionByDayServices = comsuptionByDayServices;
        }

        [HttpGet, Authorize("Reporter, AdminRequired, Reader")]
        public ActionResult<ResultPattern<Paging<CalculatedComsuptionReport>>> GetCalculatedComsuptionByDay([FromQuery] GridifyQuery query)
            => Ok(_comsuptionByDayServices.GetAll(query));

    }
}
