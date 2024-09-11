using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Gridify;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ActualStockServices : GenericRepository<vw_ActualStock>, IActualStockServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISAPService sAPService;
        public ActualStockServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor, ISAPService sAPService)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
            this.sAPService = sAPService;
        }
        public override ResultPattern<vw_ActualStock> Get(Func<vw_ActualStock, bool> predicate)
            => base.Get(predicate);
        

        public override ResultPattern<Paging<vw_ActualStock>> GetAll(GridifyQuery query)
        {

            var warehouseParameter = Regex.Match(query.Filter ?? "", $"({nameof(vw_ActualStock.WareHouseId)})=[0-9]+", RegexOptions.IgnoreCase)?.Value;

            if (string.IsNullOrWhiteSpace(warehouseParameter))
                throw new BadRequestException("WarehouseId is required.");

            var warehouseId = int.Parse(warehouseParameter.Split("=")[1]);

            var warehouse = _DBContext.WareHouse.Find(warehouseId)
                ?? throw new NotFoundException("Warehouse not found");

            return GetSapStock(warehouse);
        }

        public ResultPattern<Paging<vw_ActualStock>> GetSapStock(WareHouse warehouse)
        {
            try
            {

                var items = _DBContext.ArticleDataMaster.Where(x => x.CompanyId == warehouse.CompanyId).ToList();

                var getArticleTask = sAPService.GetItemsStockSAP(warehouse.Code ?? "", items.Select(x => x.ArticleNumber ?? "").ToArray());
                getArticleTask.Wait();
                var sapResponse = getArticleTask.Result
                    ?? throw new BadRequestException(getArticleTask?.Exception?.Message ?? "");

                var stock = sapResponse.Value.Select(item => new vw_ActualStock()
                {
                    ItemId = items.FirstOrDefault(x => x.ArticleNumber == item.Items?.ItemCode)?.Id ?? 0,
                    ArticleCode = item.Items?.ItemCode,
                    ArticleDescription = item.Items?.ItemName,
                    StockQty = item.StockInfo?.InStock ?? 0,
                    WareHouseCode = warehouse.Code,
                    WareHouseId = warehouse.Id!,
                    WareHouseName = warehouse.Name,
                    BranchOfficeId = warehouse.BranchOfficeId ?? 0,
                    BranchOfficeName = warehouse.BranchOffice?.Name,
                    CompanyId = warehouse.CompanyId ?? 0,
                    CompanyName = warehouse.Company?.Name
                });


                return ResultPattern<Paging<vw_ActualStock>>.Success(new Paging<vw_ActualStock>
                {
                    Data = stock.ToList(),
                    Count = stock.Count(),
                }, StatusCodes.Status200OK, "Stock obtained successfully.");
            }
            catch (Exception ex)
            {
                return ResultPattern<Paging<vw_ActualStock>>.Failure(StatusCodes.Status400BadRequest, "No stock.");
            }
        }

    }
}
