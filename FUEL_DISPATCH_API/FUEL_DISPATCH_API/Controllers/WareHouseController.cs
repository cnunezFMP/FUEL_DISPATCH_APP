using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WareHouseController : ControllerBase
    {
        private readonly IStoreServices _storeServices;
        public WareHouseController(IStoreServices storeServices)
        {
            _storeServices = storeServices;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<WareHouseMovement>>> GetStores([FromQuery] GridifyQuery query)
        {
            return Ok(_storeServices.GetAll(query));
        }
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<WareHouseMovement>> GetStore(int id)
        {
            return Ok(_storeServices.Get(x => x.Id == id));
        }
        [HttpPost]
        public ActionResult<ResultPattern<WareHouseMovement>> PostStore([FromBody] WareHouseMovement store)
        {
            return CreatedAtAction(nameof(GetStore), new { id = store.Id }, _storeServices.Post(store));
        }
        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<WareHouseMovement>> UpdateStore(int id, [FromBody] WareHouseMovement store)
        {
            return Ok(_storeServices.Update(x => x.Id == id, store));
        }
    }
}
