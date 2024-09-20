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
        //private readonly IValidator<UserRegistrationDto> _userValidator;
        private readonly IUsersAuth _usersAuth;
        public AuthController(/*IValidator<UserRegistrationDto> userValidator*/ IUsersAuth usersAuth)
        {
            // _userValidator = userValidator;
            _usersAuth = usersAuth;
        }

        [HttpPost("Register")]
        public ActionResult<ResultPattern<UserRegistrationDto>> Register([FromBody] UserRegistrationDto user)
            => Created(string.Empty, _usersAuth.UserRegistration(user));

        [HttpPost("Login")]
        public ActionResult<ResultPattern<LoginDto>> Login([FromBody] LoginDto loginDto)
            => Ok(_usersAuth.Login(loginDto));

        [HttpPut("ChangePassword/{userid}")]
        public ActionResult<bool> ChangePassword(int userid, [FromBody] ChangeUserPasswordDto changeUserPasswordDto)
        {
            bool predicate(User x) => x.Id == userid;
            return Ok(_usersAuth.ChangePassword(predicate, changeUserPasswordDto));
        }

    }
}