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
                SetCompanyId(wareHouse);
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
        public WareHouseServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }
        public bool WareHouseExists(WareHouse wareHouse)
            => !_DBContext.WareHouse.Any(x => x.Code == wareHouse.Code);
        public bool SetCompanyId(WareHouse wareHouse)
        {
            var branchOffice = _DBContext.BranchOffices.FirstOrDefault(x => x.Id == wareHouse.BranchOfficeId);
            wareHouse.CompanyId = branchOffice!.CompanyId;
            return true;
        }
        public bool BranchOfficeExist(WareHouse wareHouse)
            => !_DBContext.BranchOffices.Any(x => x.Id == wareHouse.Id);
        public bool SetWareHouseDir(WareHouse wareHouse)
        {
            var branchOffice = _DBContext.BranchOffices.FirstOrDefault(x => x.Id == wareHouse.BranchOfficeId);
            wareHouse.FullDirection = branchOffice!.FullLocation;
            return true;
        }
    }
}
