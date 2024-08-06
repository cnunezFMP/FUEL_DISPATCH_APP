using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class BranchIslandServices : GenericRepository<BranchIsland>, IBranchIslandServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchIslandServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public bool BranchIslandCodeMustBeUnique(BranchIsland branchIsland)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            return (from t0 in _DBContext.BranchIslands
                    join t1 in _DBContext.BranchOffices on t0.BranchOfficeId equals int.Parse(branchId)
                    join t2 in _DBContext.Companies on t1.CompanyId equals int.Parse(companyId)
                    where t0.Code == branchIsland.Code
                    select t0)
                    .Any();
        }
    }
}
