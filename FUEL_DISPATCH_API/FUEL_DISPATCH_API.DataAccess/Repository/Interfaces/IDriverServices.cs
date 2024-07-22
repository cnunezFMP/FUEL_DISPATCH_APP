using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IDriversServices : IGenericInterface<Driver>
    {
        ResultPattern<List<WareHouseMovement>> GetDriverDispatches(int driverId);
        bool CheckIfIdIsUnique(Driver entity);
        bool IsEmailUnique(Driver driver);
    }
}
