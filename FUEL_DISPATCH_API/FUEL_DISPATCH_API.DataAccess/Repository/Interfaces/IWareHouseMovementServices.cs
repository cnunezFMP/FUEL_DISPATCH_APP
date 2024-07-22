using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IWareHouseMovementServices : IGenericInterface<WareHouseMovement>
    {
        bool CheckPreviousVehicleDispatch(WareHouseMovement wareHouseMovement);
        bool CheckVehicle(int vehicleId);
        bool QtyCantBeZero(WareHouseMovement wareHouseMovement);
        bool CheckDriver(WareHouseMovement wareHouseMovement);
        bool CheckBranchOffice(WareHouseMovement wareHouseMovement);
        bool CheckDispenser(WareHouseMovement wareHouseMovement);
        bool SetDriverIdByVehicle(WareHouseMovement wareHouseMovement);
        bool CheckWareHouseStock(WareHouseMovement wareHouseMovement);
        bool CheckIfProductIsInTheWareHouse(WareHouseMovement wareHouseMovement);
        bool CheckIfWareHousesHasActiveStatus(WareHouseMovement wareHouseMovement);
        bool WillStockFallBelowMinimum(WareHouseMovement wareHouseMovement);
        bool WillStockFallMaximun(WareHouseMovement wareHouseMovement);
        bool SetRequestForMovement(WareHouseMovement wareHouseMovement);
    }
}
