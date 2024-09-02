using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class BranchOfficeServices : GenericRepository<BranchOffices>, IBranchOfficeServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BranchOfficeServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
        }

        public override ResultPattern<BranchOffices> Post(BranchOffices entity)
        {
            if (BranchCodeMustBeUnique(entity))
                throw new BadRequestException("Existe una sucursal con el mismo codigo. ");

            return base.Post(entity);
        }

        public bool BranchCodeMustBeUnique(BranchOffices branchOffice)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            return _DBContext.BranchOffices
                .Any(x => x.Code == branchOffice.Code &&
                x.CompanyId == int.Parse(companyId));
        }

    }
}
