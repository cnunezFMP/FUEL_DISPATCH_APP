using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IWareHouseHistoryServices : IGenericInterface<vw_WareHouseHistory>
    {
        ResultPattern<List<vw_WareHouseHistory>> GetHistoryFromSpecificWareHouse(int wareHouseId);
    }
}