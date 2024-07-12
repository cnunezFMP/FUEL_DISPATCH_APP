using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.Utils.ResponseObjects;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Interfaces
{
    public interface IEmployeeComsuptionLimits : IGenericInterface<EmployeeConsumptionLimits>
    {
        ResultPattern<EmployeeConsumptionLimits> DeleteDriverMethod(int driverId, int methodId);
        // ResultPattern<EmployeeConsumptionLimits> UpdateDriverMethod(int driverId, int methodId);

    }
}
