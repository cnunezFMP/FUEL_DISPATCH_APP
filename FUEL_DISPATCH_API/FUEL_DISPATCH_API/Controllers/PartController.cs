
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        // Hola mundo 
        [HttpGet, Authorize("")]
        public ActionResult<ResultPattern<Paging<Part>>> GetParts([FromQuery] GridifyQuery query)
        {
            return Ok(_partServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize]
        public ActionResult<ResultPattern<Part>> GetPart(int id)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(Part x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);

            return Ok(_partServices.Get(predicate));
        }

        /// <summary>
        /// Crear un nuevo articulo en un almacen. 
        /// </summary>
        /// <param name="part"></param>
        /// <response code="201">Si se crea el articulo correctamente. </response>
        /// <response code="400">Si se intenta agregar un articulo con el codigo de una ya existente. </response>
        /// <response code="400">Si se envia el numero de articulo nulo. </response>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<Part>> PostPart([FromBody] Part part)
        {
            //var validationResult = _validator.Validate(part);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}

            return Created(string.Empty, _partServices.Post(part));
        }

        [HttpPut("{id:int}"), Authorize()]
        public ActionResult<ResultPattern<Part>> UpdatePart(int id, [FromBody] Part part)
        {
            //var validationResult = _validator.Validate(part);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}

            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            bool predicate(Part x) => x.Id == id &&
                                      x.CompanyId == int.Parse(companyId);
            return Ok(_partServices.Update(predicate, part));
        }
    }
}
