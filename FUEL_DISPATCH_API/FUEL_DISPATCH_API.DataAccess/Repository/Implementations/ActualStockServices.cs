using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ActualStockServices : GenericRepository<vw_ActualStock>, IActualStockServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ActualStockServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public ResultPattern<List<vw_ActualStock>> GetWareHouseArticles(int wareHouseId, int? articleId)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var wareHouse = _DBContext.WareHouse.FirstOrDefault(x => x.Id == wareHouseId &&
            x.CompanyId == int.Parse(companyId) &&
            x.BranchOfficeId == int.Parse(branchId))
                ?? throw new NotFoundException("No warehouse found. ");

            if (articleId.HasValue)
            {
                var article = _DBContext.ArticleDataMaster.Find(articleId)
                    ?? throw new NotFoundException("This article doesn't exist. ");

                var actualStockFromWareHouseWithArticleId = _DBContext.vw_ActualStock
                .Where(x => x.WareHouseId == wareHouseId
                && x.ItemId == articleId)
                .ToList();

            }

            var actualStockFromWareHouse = _DBContext.vw_ActualStock
                .Where(x => x.WareHouseId == wareHouseId)
                .ToList();

            return ResultPattern<List<vw_ActualStock>>
                .Success(actualStockFromWareHouse,
                StatusCodes.Status200OK,
                "Actual stock from warehouse obtained. ");
        }
    }
}
