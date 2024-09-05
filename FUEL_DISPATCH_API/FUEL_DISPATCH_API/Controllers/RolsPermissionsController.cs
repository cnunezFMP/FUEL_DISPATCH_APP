using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEL_DISPATCH_API.Controllers
{
    [ApiController, Route("api/[controller]"), Authorize(Policy = "AdminRequired")]
    public class RolsPermissionsController : ControllerBase
    {
        private readonly IRolsPermissionsServices rolsPermissionsServices;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RolsPermissionsController(IRolsPermissionsServices rolsPermissionsServices,
            IHttpContextAccessor httpContextAccessor)
        {
            this.rolsPermissionsServices = rolsPermissionsServices;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet, Authorize]
        public ActionResult<ResultPattern<Paging<RolsPermissions>>> GetRolsPermissions([FromQuery] GridifyQuery query)
            => Ok(rolsPermissionsServices.GetAll(query));


        [HttpGet("{rolid:int}"), Authorize]
        public ActionResult<ResultPattern<RolsPermissions>> GetRolPermission(int rolid)
        {
            string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();
            bool predicate(RolsPermissions x) => x.RolId == rolid &&
                                                 x.CompanyId == int.Parse(companyId);
            return Ok(rolsPermissionsServices.Get(predicate));
        }

        /// <summary>
        /// Crear un nueva parte de vehiculo. 
        /// </summary>
        /// <param name="rolsPermissions"></param>
        /// <response code="201">Si se crea el articulo correctamente. </response>
        /// <response code="400">Si se intenta agregar un articulo con el codigo de una ya existente. </response>
        /// <response code="400">Si se envia el numero de articulo nulo. </response>
        /// <returns></returns>
        [HttpPost, Authorize]
        public ActionResult<ResultPattern<RolsPermissions>> PostRolPermission([FromBody] RolsPermissions rolsPermissions)
            => Created(string.Empty, rolsPermissionsServices.Post(rolsPermissions));

        [HttpPut("{rolId:int}"), Authorize]
        public ActionResult<ResultPattern<RolsPermissions>> UpdateRolPermission(int rolId, [FromBody] RolsPermissions rolsPermissions)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            bool predicate(RolsPermissions x) => x.RolId == rolId &&
                x.CompanyId == int.Parse(companyId);

            return Ok(rolsPermissionsServices.Update(predicate, rolsPermissions));
        }

        [HttpDelete("{rolId:int}/Permissions/{permissionId:int}")]
        public ActionResult<ResultPattern<RolsPermissions>> DeleteRolPermissin(int permissionId, int rolId)
        {
            string? companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();


            bool predicate(RolsPermissions x) => x.RolId == rolId &&
            x.PermissionId == permissionId &&
            x.CompanyId == int.Parse(companyId);


            return Ok(rolsPermissionsServices.Delete(predicate));
        }


    }
}
