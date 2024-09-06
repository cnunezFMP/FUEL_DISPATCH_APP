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

        public ResultPattern<List<vw_WareHouseHistory>> GetHistoryFromSpecificWareHouse(int wareHouseId)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            var history = _DBContext.Vw_WareHouseHistories
                .Where(x => x.WareHouse == wareHouseId /*&&
                       x.CompanyId == int.Parse(companyId) &&
                       x.BranchOfficeId == int.Parse(branchId)*/)
                .ToList();

            return ResultPattern<List<vw_WareHouseHistory>>.Success(history, StatusCodes.Status200OK, "History from specific warehouse retrieved successfully."); 
        }
    }
}
