using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Implementations;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IEmployeeComsuptionLimitsServices : IGenericInterface<EmployeeConsumptionLimits>
    {
        ResultPattern<Driver> DeleteDriverMethod(int driverId, int methodId);
        ResultPattern<Driver> UpdateDriverMethod(int driverId, int methodId, EmployeeConsumptionLimits employeeConsumptionLimit);
        bool DriverHasTheMethod(int driverId, int methodOfComsuptionId);
        // ResultPattern<EmployeeConsumptionLimits> UpdateDriverMethod(int driverId, int methodId);

    }
}
