using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IActualStockServices : IGenericInterface<vw_ActualStock>
    {
        ResultPattern<List<vw_ActualStock>> GetWareHouseArticles(int wareHouseId, int? articleId);
    }
}
