using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DispenserServices : GenericRepository<Dispenser>, IDispenserServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DispenserServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
        }
        public override ResultPattern<Dispenser> Post(Dispenser entity)
        {
            if (DispenserCodeMustBeUnique(entity))
                throw new BadRequestException("Existe un dispensador con el mismo codigo. ");
            return base.Post(entity);
        }
        public bool DispenserCodeMustBeUnique(Dispenser dispenser)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            return _DBContext.Dispenser
                .Any(x => x.Code == dispenser.Code /*&&
                     x.CompanyId == int.Parse(companyId) &&
                     x.BranchOfficeId == int.Parse(branchId)*/);

        }

    }
}
