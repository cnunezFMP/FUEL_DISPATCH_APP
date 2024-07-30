using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    // DONE: un endpoint donde yo especifique el almacen y me traiga todos los articulos de este, este tendria como parametron el almacen y el tipo de articulo.
    [ApiController]
    [Route("api/[controller]")]
    public class vw_ActualStockController : ControllerBase
    {
        private readonly IActualStockServices _actualStockServices;
        public vw_ActualStockController(IActualStockServices actualStockServices)
        {
            _actualStockServices = actualStockServices;
        }
        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<vw_ActualStock>>> GetVwActualStock([FromQuery] GridifyQuery query)
        {
            string? companyId, branchId;
            GetUserCompanyAndBranchClass.GetUserCompanyAndBranch(out companyId, out branchId);
            return Ok(_actualStockServices.GetAll(query));
        }
        [HttpGet("{warehouseId:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<vw_WareHouseHistory>>> GetVwWareHouseHistory(int warehouseId, int? articleId)
        {
            return Ok(_actualStockServices.GetWareHouseArticles(warehouseId, articleId));
        }
    }
}
