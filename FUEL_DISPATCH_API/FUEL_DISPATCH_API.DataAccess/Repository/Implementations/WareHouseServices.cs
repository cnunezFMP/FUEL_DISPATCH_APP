using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseServices : GenericRepository<WareHouse>, IWareHouseServices
    {
        public override ResultPattern<WareHouse> Post(WareHouse wareHouse)
        {
            if (wareHouse.BranchOfficeId.HasValue)
            {
                SetWareHouseDir(wareHouse);
            }
            _DBContext.WareHouse.Add(wareHouse);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouse>.Success(
                wareHouse,
                StatusCodes.Status201Created,
                "Warehouse added successfully. ");
        }
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WareHouseServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
        }
        public bool WareHouseExists(WareHouse wareHouse)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            return !_DBContext.WareHouse.Any(x => x.Code == wareHouse.Code &&
            x.CompanyId == int.Parse(companyId) &&
            x.BranchOfficeId == int.Parse(branchId));
        }

        public bool BranchOfficeExist(WareHouse wareHouse)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            return _DBContext.BranchOffices.Any(x => x.Id == wareHouse.BranchOfficeId &&
            x.CompanyId == int.Parse(companyId));
        }

        public bool SetWareHouseDir(WareHouse wareHouse)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            var branchOffice = _DBContext.BranchOffices
                .FirstOrDefault(x => x.Id == wareHouse.BranchOfficeId &&
                x.CompanyId == int.Parse(companyId));
            wareHouse.FullDirection = branchOffice!.FullLocation;
            return true;
        }
    }
}
