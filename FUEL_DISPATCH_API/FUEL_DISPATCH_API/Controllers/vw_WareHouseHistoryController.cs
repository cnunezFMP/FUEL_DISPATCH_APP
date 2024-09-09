using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize/*(Roles = "Administrador")*/]
    public class vw_WareHouseHistoryController : ControllerBase
    {
        private readonly IWareHouseHistoryServices _wareHouseHistoryServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public vw_WareHouseHistoryController(IWareHouseHistoryServices wareHouseHistoryServices,
                                             IHttpContextAccessor httpContextAccessor)
        {
            _wareHouseHistoryServices = wareHouseHistoryServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<vw_WareHouseHistory>>> GetVwWareHouseHistory([FromQuery] GridifyQuery query) => Ok(_wareHouseHistoryServices.GetAll(query));
        
        [HttpGet("{wareHouseId:int}"), Authorize]
        public ActionResult<ResultPattern<vw_WareHouseHistory>> GetHistoryFromWareHouse(int wareHouseId)
            => Ok(_wareHouseHistoryServices.GetHistoryFromSpecificWareHouse(wareHouseId));
    }
}
