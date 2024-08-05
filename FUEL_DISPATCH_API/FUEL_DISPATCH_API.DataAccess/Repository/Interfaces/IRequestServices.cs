using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IRequestServices : IGenericInterface<WareHouseMovementRequest>
    {
        bool CheckVehicle(WareHouseMovementRequest newRequest);
        bool CheckDriver(WareHouseMovementRequest newRequest);

        bool CheckIfWareHousesHasActiveStatus(WareHouseMovementRequest wareHouseMovementRequest);
    }
}
