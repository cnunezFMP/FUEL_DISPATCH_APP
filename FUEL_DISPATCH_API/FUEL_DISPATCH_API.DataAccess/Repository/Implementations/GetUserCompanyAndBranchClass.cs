using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class GetUserCompanyAndBranchClass
    {
        private static IHttpContextAccessor? _httpContextAccessor;
        public GetUserCompanyAndBranchClass(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        // DONE: Pasarle el context accesor desde los controladores. 
        public void GetUserCompanyAndBranch(out string? companyId, out string? branchId)
        {
            var httpContext = _httpContextAccessor?.HttpContext;

            companyId = httpContext?
                .User
                .Claims
                .FirstOrDefault(x => x.Type == "CompanyId")?
                .Value;

            branchId = httpContext?
                .User
                .Claims
                .FirstOrDefault(x => x.Type == "BranchOfficeId")?
                .Value;
        }
    }
}
