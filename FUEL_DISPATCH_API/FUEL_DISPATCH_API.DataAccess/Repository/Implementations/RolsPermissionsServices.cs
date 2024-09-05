using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class RolsPermissionsServices : GenericRepository<RolsPermissions>, IRolsPermissionsServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RolsPermissionsServices(FUEL_DISPATCH_DBContext DBContext,
                                       IHttpContextAccessor httpContextAccessor) :
                                       base(DBContext, httpContextAccessor)
        {
            _DBContext = DBContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ResultPattern<RolsPermissions> Post(RolsPermissions entity)
        {
            if (RolHasThisPermission(entity.RolId, entity.PermissionId))
                throw new BadRequestException("Este rol ya tiene el permiso asignado. ");

            return base.Post(entity);
        }
        public bool RolHasThisPermission(int? rolId, int? permissionId)
        {
            string? companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();

            return _DBContext.RolsPermissions.Any(x => x.RolId == rolId &&
                       x.PermissionId == permissionId &&
                       x.CompanyId == int.Parse(companyId));
        }
    }
}
