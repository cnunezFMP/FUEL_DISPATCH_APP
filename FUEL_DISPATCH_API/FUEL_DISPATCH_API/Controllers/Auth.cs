using FMP_MATEINANCE_API.Auth;
using FUEL_DISPATCH_API.DataAccess.DTOs;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Auth : ControllerBase 
    {
        private readonly IUsersAuth _usersAuth;
        public Auth(IUsersAuth usersAuth)
        {
            _usersAuth = usersAuth;
        }   
        [HttpPost("Register")]
        public ActionResult<ResultPattern<Users>> Register([FromBody]Users user)
        {
            return Ok(_usersAuth.Post(user));
        }
        [HttpPost("Login")]
        public ActionResult<ResultPattern<Users>> Login([FromBody] LoginDto loginDto)
        {
            return Ok(_usersAuth.Login(loginDto));
        }
    }
}
