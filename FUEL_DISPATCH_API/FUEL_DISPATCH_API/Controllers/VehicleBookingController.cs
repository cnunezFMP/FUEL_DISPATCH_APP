using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleBookingController : ControllerBase
    {
        private readonly IValidator<Booking> _bookingValidator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBookingServices _bookingServices;
        public VehicleBookingController(IBookingServices bookingServices,
                                        IValidator<Booking> bookingValidator,
                                        IHttpContextAccessor httpContextAccessor)
        {
            _bookingServices = bookingServices;
            _bookingValidator = bookingValidator;
            _httpContextAccessor = httpContextAccessor;

        }
        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<Booking>>> GetBookings([FromQuery] GridifyQuery query)
        {
            return Ok(_bookingServices.GetAll(query));
        }
        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Booking>> GetBooking(int id)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(Booking x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId) &&
                                      x.BranchOfficeId == int.Parse(branchId);

            return Ok(_bookingServices.Get(predicate));
        }
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Booking>> PostBooking([FromBody] Booking booking)
        {
            var validationResult = _bookingValidator.Validate(booking);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            return Created(string.Empty, _bookingServices.Post(booking));
        }
        [HttpPut("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Booking>> UpdateBooking(int id, [FromBody] Booking booking)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            bool predicate(Booking x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId) &&
                                      x.BranchOfficeId == int.Parse(branchId);

            return Ok(_bookingServices.Update(predicate, booking));
        }
    }
}
