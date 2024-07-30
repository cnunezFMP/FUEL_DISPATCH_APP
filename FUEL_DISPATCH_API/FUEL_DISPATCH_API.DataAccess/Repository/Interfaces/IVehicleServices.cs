using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IVehiclesServices : IGenericInterface<Vehicle>
    {
        bool DriverIdHasValue(Vehicle entity);
        ResultPattern<List<WareHouseMovement>> GetVehicleDispatches(int vehicleId, string? branchId, string? companyId);
        bool CheckIfMakeExists(Vehicle vehicle);
        bool CheckIfModelExists(Vehicle vehicle);
        bool CheckIfGenerationExists(Vehicle vehicle);
        bool CheckIfModEngineExists(Vehicle vehicle);
        bool CheckIfMeasureExists(Vehicle vehicle);
        bool FichaMustBeUnique(Vehicle vehicleToken);
    }
}
