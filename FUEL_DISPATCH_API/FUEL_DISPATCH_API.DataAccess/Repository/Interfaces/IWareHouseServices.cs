using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IWareHouseServices : IGenericInterface<WareHouse>
    {
        bool WareHouseExists(WareHouse wareHouse);
        // bool BranchOfficeExist(WareHouse wareHouse);
        //bool SetWareHouseDir(WareHouse wareHouse);
    }
}