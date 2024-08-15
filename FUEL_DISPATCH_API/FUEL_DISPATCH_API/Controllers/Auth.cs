using FluentValidation;
using FluentValidation.Results;
using FUEL_DISPATCH_API.Auth;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;
namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Auth : ControllerBase
    {
        private readonly IValidator<UserRegistrationDto> _userValidator;
        private readonly IUsersAuth _usersAuth;
        public Auth(IValidator<UserRegistrationDto> userValidator, IUsersAuth usersAuth)
        {
            _userValidator = userValidator;
            _usersAuth = usersAuth;
        }
        /// <summary>
        /// Registrar un nuevo usuario
        /// </summary>
        /// <remarks>
        /// Tanto el "DriverId" como el "Email" pueden ser nulos. Las propiedades "CreatedBy" y "UpdatedBy" se quitan del JSON.
        /// </remarks>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public ActionResult<ResultPattern<User>> Register([FromBody] UserRegistrationDto user)
        {
            //var result = _userValidator.Validate(user);
            //if (!result.IsValid)
            //{
            //    var modelstateDictionary = new ModelStateDictionary();
            //    foreach (ValidationFailure validationFailure in result.Errors)
            //    {
            //        modelstateDictionary.AddModelError(validationFailure.PropertyName, validationFailure.ErrorMessage);
            //    }
            //    return ValidationProblem(modelstateDictionary);
            //}
            return Created(string.Empty, _usersAuth.UserRegistration(user));
        }
        [HttpPost("Login")]
        public ActionResult<ResultPattern<User>> Login([FromBody] LoginDto loginDto)
        {
            return Ok(_usersAuth.Login(loginDto));
        }
    }
}