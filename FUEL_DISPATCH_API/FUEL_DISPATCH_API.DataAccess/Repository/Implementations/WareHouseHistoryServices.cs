using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseHistoryServices : GenericRepository<vw_WareHouseHistory>, IWareHouseHistoryServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public WareHouseHistoryServices(FUEL_DISPATCH_DBContext dbContext,
            IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
        }
        public ResultPattern<List<vw_WareHouseHistory>> GetHistoryFromSpecifiedWarehouse(int warehouseId)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var wareHouseHistoryList = (from t0 in _DBContext.Vw_WareHouseHistories
                                        join t1 in _DBContext.WareHouse on t0.WareHouse equals t1.Id
                                        where t0.WareHouse == warehouseId &&
                                        t1.CompanyId == int.Parse(companyId) &&
                                        t1.BranchOfficeId == int.Parse(branchId)
                                        select t0)
                                       .AsNoTracking()
                                       .ToList();




            if (!wareHouseHistoryList.Any())
                throw new BadRequestException("This warehouse don't has history. ");


            return ResultPattern<List<vw_WareHouseHistory>>.Success(wareHouseHistoryList,
                StatusCodes.Status200OK,
                "History from the specified warehouse obtained. ");
        }
    }
}
