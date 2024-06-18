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
    public class RequestController : Controller
    {
        private readonly IRequestServices _requestServices;
        public RequestController(IRequestServices requestServices)
        {
            _requestServices = requestServices;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<Request>>> GetRequests([FromQuery] GridifyQuery query)
        {
            return Ok(_requestServices.GetAll(query));
        }
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Request>> GetRequest(int id)
        {
            return Ok(_requestServices.Get(x => x.Id == id));
        }

        [HttpPost]
        public ActionResult<ResultPattern<Request>> PostRequest([FromBody] Request request)
        {
            request.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            request.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetRequest), new { id = request.Id }, _requestServices.Post(request));
        }

        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<User>> UpdateRequest(int id, [FromBody] Request request)
        {
            request.UpdatedAt = DateTime.Now;
            request.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_requestServices.Update(x => x.Id == id, request));
        }
    }
}
