using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
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
        public override ResultPattern<BranchIsland> Post(BranchIsland entity)
        {
            if (BranchIslandCodeMustBeUnique(entity))
                throw new BadRequestException("Existe una isla con el mismo codigo asignado. ");

            return base.Post(entity);

        }

        public bool BranchIslandCodeMustBeUnique(BranchIsland branchIsland)
        {
            //string? companyId, branchId;
            //companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            //branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();


            return _DBContext.BranchIslands
                .Any(x => x.Code == branchIsland.Code /*&&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId)*/);
        }
    }
}
