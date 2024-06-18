using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockController : ControllerBase
    {
        private readonly IStockServices _stockServices;

        public StockController(IStockServices stockServices)
        {
            _stockServices = stockServices;
        }

        [HttpGet]
        public ActionResult<ResultPattern<Paging<Stock>>> GetStocks([FromQuery] GridifyQuery query)
        {
            return Ok(_stockServices.GetAll(query));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Stock>> GetStock(int id)
        {
            return Ok(_stockServices.Get(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult<ResultPattern<Stock>> PostStock([FromBody] Stock stock)
        {
            return CreatedAtAction(nameof(GetStock), new { id = stock.Id }, _stockServices.Post(stock));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<Stock>> UpdateUser(int id, [FromBody] Stock stock)
        {
            return Ok(_stockServices.Update(x => x.Id == id, stock));
        }
    }
}
