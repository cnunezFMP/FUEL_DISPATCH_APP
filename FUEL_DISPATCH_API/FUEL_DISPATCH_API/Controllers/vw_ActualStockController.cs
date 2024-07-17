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
    public class vw_ActualStockController : ControllerBase
    {
        private readonly IActualStockServices _actualStockServices;
        public vw_ActualStockController(IActualStockServices actualStockServices)
        {
            _actualStockServices = actualStockServices;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<vw_ActualStock>>> GetVwActualStock([FromQuery] GridifyQuery query)
        {
            return Ok(_actualStockServices.GetAll(query));
        }
    }
}
