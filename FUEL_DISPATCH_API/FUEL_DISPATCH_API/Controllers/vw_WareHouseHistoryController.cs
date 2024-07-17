using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class vw_WareHouseHistoryController : ControllerBase
    {
        private readonly IWareHouseHistoryServices _wareHouseHistoryServices;
        public vw_WareHouseHistoryController(IWareHouseHistoryServices wareHouseHistoryServices)
        {
            _wareHouseHistoryServices = wareHouseHistoryServices;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<vw_WareHouseHistory>>> GetVwWareHouseHistory([FromQuery] GridifyQuery query)
        {
            return Ok(_wareHouseHistoryServices.GetAll(query));
        }
    }
}
