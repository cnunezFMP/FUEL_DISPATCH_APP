using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
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
        private readonly IBookingServices _bookingServices;
        public VehicleBookingController(IBookingServices bookingServices, IValidator<Booking> bookingValidator)
        {
            _bookingServices = bookingServices;
            _bookingValidator = bookingValidator;
        }
        [HttpGet]
        public ActionResult<ResultPattern<Paging<Booking>>> GetBookings([FromQuery] GridifyQuery query)
        {
            return Ok(_bookingServices.GetAll(query));
        }
        [HttpGet("{id:int}")]
        public ActionResult<ResultPattern<Booking>> GetBooking(int id)
        {
            return Ok(_bookingServices.Get(x => x.Id == id));
        }
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Booking>> PostBooking([FromBody] Booking booking)
        {
            var validationResult = _bookingValidator.Validate(booking);
            if (!validationResult.IsValid)
            {
                return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            }
            booking.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            booking.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, _bookingServices.Post(booking));
        }
        [HttpPut("{id:int}")]
        public ActionResult<ResultPattern<Booking>> UpdateBooking(int id, [FromBody] Booking booking)
        {
            booking.UpdatedAt = DateTime.Now;
            booking.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            return Ok(_bookingServices.Update(x => x.Id == id, booking));
        }
    }
}
