using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseServices : GenericRepository<WareHouse>, IWareHouseServices
    {

        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISAPService _sapService;
        public WareHouseServices(FUEL_DISPATCH_DBContext dbContext,
                                 IHttpContextAccessor httpContextAccessor,
                                 ISAPService sapService)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
            _sapService = sapService;
        }

        public override ResultPattern<WareHouse> Post(WareHouse wareHouse)
        {
            try
            {
                var getWareHouseTask = _sapService.GetWarehouseSAP(wareHouse.Code);
                getWareHouseTask.Wait();
            }
            catch (Exception ex)
            {
                return ResultPattern<WareHouse>.Failure(
                    StatusCodes.Status400BadRequest,
                    "The warehouse doesn't exist in SAP");
            }

            /*if (wareHouse.BranchOfficeId.HasValue)
            {
                SetWareHouseDir(wareHouse);
            }*/
            return base.Post(wareHouse);
        }
        public bool WareHouseExists(WareHouse wareHouse)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            return !_DBContext.WareHouse.Any(x => x.Code == wareHouse.Code /*&&
            x.CompanyId == int.Parse(companyId) &&
            x.BranchOfficeId == int.Parse(branchId)*/);
        }

        /*public bool BranchOfficeExist(WareHouse wareHouse)
        {
            string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();

            return _DBContext
                .BranchOffices
                .Any(x => x.Id == wareHouse.BranchOfficeId &&
                x.CompanyId == int.Parse(companyId));
        }*/

        /*public bool SetWareHouseDir(WareHouse wareHouse)
        {
            *//*string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();*//*

            var branchOffice = _DBContext.BranchOffices
                .FirstOrDefault(x => x.Id == wareHouse.BranchOfficeId &&
                x.CompanyId == int.Parse(companyId)) ??
                throw new NotFoundException("La sucursal indicada no existe. ");
            wareHouse.FullDirection = branchOffice!.FullLocation;
            return true;
        }*/
    }
}