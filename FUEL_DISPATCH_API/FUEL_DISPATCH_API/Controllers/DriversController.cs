using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriversController : ControllerBase
    {
        private readonly IDriversServices _driverServices;

        public DriversController(IDriversServices driverServices)
        {
            _driverServices = driverServices;
        }

        [HttpGet]
        public ActionResult<ResultPattern<Paging<Driver>>> GetDrivers([FromQuery] GridifyQuery query)
        {
            return Ok(_driverServices.GetAll(query));
        }

        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Driver>> GetDriver(int id)
        {
            return Ok(_driverServices.Get(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult<ResultPattern<Driver>> PostDriver([FromBody] Driver driver)
        {
            return CreatedAtAction(nameof(GetDriver), new { id = driver.Id }, _driverServices.Post(driver));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<User>> UpdateUser(int id, [FromBody] Driver driver)
        {
            return Ok(_driverServices.Update(x => x.Id == id, driver));
        }
    }
}
