using FluentValidation;
using FluentValidation.Results;
using FMP_MATEINANCE_API.Auth;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Twilio.TwiML.Voice;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Auth : ControllerBase
    {
        private readonly IValidator<User> _userValidator;
        private readonly IUsersAuth _usersAuth;
        public Auth(IValidator<User> userValidator, IUsersAuth usersAuth)
        {
            _userValidator = userValidator;
            _usersAuth = usersAuth;
        }
        [HttpPost("Register")]
        public ActionResult<ResultPattern<User>> Register([FromBody] User user)
        {
            var result = _userValidator.Validate(user);
            if (!result.IsValid)
            {
                var modelstateDictionary = new ModelStateDictionary();
                foreach (ValidationFailure validationFailure in result.Errors)
                {
                    modelstateDictionary.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
                }
                return ValidationProblem(modelstateDictionary);
            }
            return Ok(_usersAuth.Post(user));
        }
        [HttpPost("Login")]
        public ActionResult<ResultPattern<User>> Login([FromBody] LoginDto loginDto)
        {
            return Ok(_usersAuth.Login(loginDto));
        }
    }
}
