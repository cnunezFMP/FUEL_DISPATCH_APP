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
        private readonly IPartServices _partServices;
        public PartController(IPartServices partServices)
        {
            _partServices = partServices;
        }

        [HttpGet, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Paging<Part>>> GetParts([FromQuery] GridifyQuery query)
        {
            return Ok(_partServices.GetAll(query));
        }

        [HttpGet("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Part>> GetPart(int id)
        {
            return Ok(_partServices.Get(x => x.Id == id));
        }

        /// <summary>
        /// Crear un nuevo articulo en un almacen. 
        /// </summary>
        /// <param name="part"></param>
        /// <response code="201">Si se crea el articulo correctamente. </response>
        /// <response code="400">Si se intenta agregar un articulo con el codigo de una ya existente. </response>
        /// <response code="400">Si se envia el numero de articulo nulo. </response>
        /// <returns></returns>
        [HttpPost, Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Part>> PostPart([FromBody] Part part)
        {
            //var validationResult = _validator.Validate(part);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            part.CreatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            part.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return CreatedAtAction(nameof(GetPart), new { id = part.Id }, _partServices.Post(part));
        }

        [HttpPut("{id:int}"), Authorize(Roles = "Administrator")]
        public ActionResult<ResultPattern<Part>> UpdatePart(int id, [FromBody] Part part)
        {
            //var validationResult = _validator.Validate(part);
            //if (!validationResult.IsValid)
            //{
            //    return ValidationProblem(ModelStateResult.GetModelStateDic(validationResult));
            //}
            part.UpdatedAt = DateTime.Now;
            part.UpdatedBy = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(_partServices.Update(x => x.Id == id, part));
        }
    }
}
