using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseHistoryServices : GenericRepository<vw_WareHouseHistory>, IWareHouseHistoryServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public WareHouseHistoryServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }

        public ResultPattern<List<vw_WareHouseHistory>> GetHistoryFromSpecifiedWarehouse(int warehouseId)
        {
            var wareHouseHistoryList = _DBContext.Vw_WareHouseHistories
                .Where(x => x.WareHouse == warehouseId)
                .ToList();

            if (!wareHouseHistoryList.Any())
                throw new BadRequestException("This warehouse don't has history. ");


            return ResultPattern<List<vw_WareHouseHistory>>.Success(wareHouseHistoryList,
                StatusCodes.Status200OK,
                "History from the specified warehouse obtained. ");
        }
    }
}
