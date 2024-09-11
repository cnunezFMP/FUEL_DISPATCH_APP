using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace FUEL_DISPATCH_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PartController : ControllerBase
    {
        private HttpContext? _httpContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPartServices _partServices;
        public PartController(IPartServices partServices,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
            _partServices = partServices;
            _httpContextAccessor = httpContextAccessor;
        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Part>>> GetParts([FromQuery] GridifyQuery query)
            => Ok(_partServices.GetAll(query));

        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Part>> GetPart(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();

            bool predicate(Part x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);
            return Ok(_partServices.Get(predicate));
        }
        [HttpPost, Authorize/*(Roles = "CanCreate, Administrador")*/]
        public ActionResult<ResultPattern<Part>> PostPart([FromBody] Part part)
           => Created(string.Empty, _partServices.Post(part));
        [HttpPut("{id:int}"), Authorize/*(Roles = "CanUpdateData, Administrador")*/]
        public ActionResult<ResultPattern<Part>> UpdatePart(int id, [FromBody] Part part)
        {
            bool predicate(Part x) => x.Id == id /* &&
                                      x.CompanyId == int.Parse(companyId)*/;

            return Ok(_partServices.Update(predicate, part));
        }
    }
}
