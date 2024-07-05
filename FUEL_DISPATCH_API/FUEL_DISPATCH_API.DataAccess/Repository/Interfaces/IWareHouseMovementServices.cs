using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IWareHouseMovementServices : IGenericInterface<WareHouseMovement>
    {
        bool CheckPreviousVehicleDispatch(WareHouseMovement wareHouseMovement);
        bool CheckVehicle(WareHouseMovement wareHouseMovement);
        bool QtyCantBeZero(WareHouseMovement wareHouseMovement);
        bool CheckDriver(WareHouseMovement wareHouseMovement);
        bool CheckBranchOffice(WareHouseMovement wareHouseMovement);
        bool CheckDispenser(WareHouseMovement wareHouseMovement);
        bool SetDriverIdByVehicle(WareHouseMovement wareHouseMovement);
        bool CheckWareHouseStock(WareHouseMovement wareHouseMovement);
        bool CheckIfProductIsInTheWareHouse(WareHouseMovement wareHouseMovement);
        bool CheckIfWareHousesHasActiveStatus(WareHouseMovement wareHouseMovement);
        bool WillStockFallBelowMinimum(WareHouseMovement wareHouseMovement);
    }
}
