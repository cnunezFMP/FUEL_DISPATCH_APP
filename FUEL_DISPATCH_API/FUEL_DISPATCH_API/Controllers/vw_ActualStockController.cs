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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IActualStockServices _actualStockServices;

        public vw_ActualStockController(IActualStockServices actualStockServices,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _actualStockServices = actualStockServices;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<vw_ActualStock>>> GetAllVwActualStock([FromQuery] GridifyQuery query)
            => Ok(_actualStockServices.GetAll(query));

        [HttpGet("{warehouseId:int}"), Authorize]
        public ActionResult<ResultPattern<Paging<vw_ActualStock>>> GetVwActualStock(int warehouseId)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(vw_ActualStock x) =>
                x.WareHouseId == warehouseId &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId);

            return Ok(_actualStockServices.Get(predicate));
        }
    }
}
