using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    // DONE: un endpoint donde yo especifique el almacen y me traiga todo el historial de este, este tendria como parametron el almacen.
    [ApiController]
    [Route("api/[controller]")]
    public class vw_WareHouseHistoryController : ControllerBase
    {
        private readonly IWareHouseHistoryServices _wareHouseHistoryServices;
        public vw_WareHouseHistoryController(IWareHouseHistoryServices wareHouseHistoryServices)
        {
            _wareHouseHistoryServices = wareHouseHistoryServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<vw_WareHouseHistory>>> GetVwWareHouseHistory([FromQuery] GridifyQuery query)
        {
            return Ok(_wareHouseHistoryServices.GetAll(query));
        }

        [HttpGet("{wareHouseId:int}")]
        public ActionResult<ResultPattern<vw_WareHouseHistory>> GetHistoryFromWareHouse(int wareHouseId)
        {
            return Ok(_wareHouseHistoryServices.GetHistoryFromSpecifiedWarehouse(wareHouseId));
        }
    }
}
