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
        public RolsPermissionsServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : base(DBContext, httpContextAccessor)
        {
            _DBContext = DBContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ResultPattern<RolsPermissions> Post(RolsPermissions entity)
        {
            RolHasThisPermission(entity);
            return base.Post(entity);
        }
        public bool RolHasThisPermission(RolsPermissions rolsPermissions)
        {
            string? companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            var rolPermission = _DBContext.RolsPermissions
                .FirstOrDefault(x => x.RolId == rolsPermissions.RolId &&
                x.PermissionId == rolsPermissions.PermissionId &&
                x.CompanyId == int.Parse(companyId));

            if (rolPermission is null)
                return false;

            if (rolPermission is not null)
                throw new BadRequestException("Este rol ya tiene este permiso asignado. ");

            return true;
        }
    }
}
