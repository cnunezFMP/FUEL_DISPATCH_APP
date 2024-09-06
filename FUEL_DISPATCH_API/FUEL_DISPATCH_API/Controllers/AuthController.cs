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
    public class AuthController : ControllerBase
    {
        private readonly IValidator<UserRegistrationDto> _userValidator;
        private readonly IUsersAuth _usersAuth;
        public AuthController(IValidator<UserRegistrationDto> userValidator, IUsersAuth usersAuth)
        {
            _userValidator = userValidator;
            _usersAuth = usersAuth;
        }
        
        [HttpPost("Register")]
        public ActionResult<ResultPattern<User>> Register([FromBody] UserRegistrationDto user)
            => Created(string.Empty, _usersAuth.UserRegistration(user));

        [HttpPost("Login")]
        public ActionResult<ResultPattern<User>> Login([FromBody] LoginDto loginDto)
            => Ok(_usersAuth.Login(loginDto));

    }
}