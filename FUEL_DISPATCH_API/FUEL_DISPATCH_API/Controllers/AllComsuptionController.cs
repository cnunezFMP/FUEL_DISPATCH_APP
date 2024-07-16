using FluentValidation;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Validators;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllComsuptionController : ControllerBase
    {
        private readonly IAllComsuptionServices _allComsuptionServices;
        public AllComsuptionController(IAllComsuptionServices allComsuptionServices)
        {
            _allComsuptionServices = allComsuptionServices;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<AllComsuption>>> GetAllComsuption([FromQuery] GridifyQuery query)
        {
            return Ok(_allComsuptionServices.GetAll(query));
        }
    }
}
