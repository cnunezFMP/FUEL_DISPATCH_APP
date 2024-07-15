using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IEmployeeComsuptionLimits : IGenericInterface<EmployeeConsumptionLimits>
    {
        ResultPattern<Driver> DeleteDriverMethod(int driverId, int methodId);
        ResultPattern<Driver> UpdateDriverMethod(int driverId, int methodId);
        bool DriverHasTheMethod(Driver driver, DriverMethodOfComsuption driverMethodOfComsuption);
        // ResultPattern<EmployeeConsumptionLimits> UpdateDriverMethod(int driverId, int methodId);

    }
}
