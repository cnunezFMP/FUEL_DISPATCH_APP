using FluentValidation;
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
    public class DispenserController : ControllerBase
    {
        private readonly IValidator<Dispenser> _dispenserValidator;
        private readonly IDispenserServices _dispenserServices;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DispenserController(IDispenserServices dispenserServices,
                                   IValidator<Dispenser> dispenserValidator,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _dispenserServices = dispenserServices;
            _dispenserValidator = dispenserValidator;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Dispenser>>> GetDispensers([FromQuery] GridifyQuery query)
            => Ok(_dispenserServices.GetAll(query));

        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Dispenser>> GetDispenser(int id)
        {
            bool predicate(Dispenser x) => x.Id == id;
            return Ok(_dispenserServices.Get(predicate));
        }

        [HttpPost, Authorize/*(Roles = "CanCreate, Administrador")*/]
        public ActionResult<ResultPattern<Dispenser>> PostDispenser([FromBody] Dispenser dispenser)
            => Created(string.Empty, _dispenserServices.Post(dispenser));


        [HttpPut("{id:int}"), Authorize/*(Roles = "CanUpdateData, Administrador")*/]
        public ActionResult<ResultPattern<Dispenser>> UpdateDispenser(int id, [FromBody] Dispenser dispenser)
        {
            bool predicate(Dispenser x) => x.Id == id;
            return Ok(_dispenserServices.Update(predicate, dispenser));
        }
    }
}
