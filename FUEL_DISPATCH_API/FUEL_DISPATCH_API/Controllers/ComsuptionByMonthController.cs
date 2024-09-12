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
    public class ComsuptionByMonthController : ControllerBase
    {

        private readonly IComsuptionByMonthServices _comsuptionByMonthServices;
        public ComsuptionByMonthController(IComsuptionByMonthServices comsuptionByMonthServices)
        {
            _comsuptionByMonthServices = comsuptionByMonthServices;
        }
        // DONE: Implementar los controladores para obtener los consumos restantes. 
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<CalculatedComsuptionReport>>> GetCalculatedComsuptionByMonth([FromQuery] GridifyQuery query)
            => Ok(_comsuptionByMonthServices.GetAll(query));
    }
}
