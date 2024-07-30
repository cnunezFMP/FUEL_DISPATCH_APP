using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class EmployeeComsuptionLimitsServices : GenericRepository<EmployeeConsumptionLimits>, IEmployeeComsuptionLimitsServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public EmployeeComsuptionLimitsServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }
        public override ResultPattern<EmployeeConsumptionLimits> Delete(Func<EmployeeConsumptionLimits, bool> predicate)
        {
            var employeeComsuptionLimitEntityToDelete = _DBContext.EmployeeConsumptionLimits
                .FirstOrDefault(predicate);

            _DBContext.EmployeeConsumptionLimits.Remove(employeeComsuptionLimitEntityToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<EmployeeConsumptionLimits>.Success(employeeComsuptionLimitEntityToDelete!, StatusCodes.Status200OK, "Driver method deleted. ");
        }

        // DONE: Actualizar.
        public override ResultPattern<EmployeeConsumptionLimits> Update(Func<EmployeeConsumptionLimits, bool> predicate, EmployeeConsumptionLimits updatedEntity)
        {
            var employeeComsuptionLimitEntity = _DBContext.EmployeeConsumptionLimits
                .FirstOrDefault(predicate);

            _DBContext.Entry(employeeComsuptionLimitEntity!).CurrentValues.SetValues(updatedEntity);
            _DBContext.SaveChanges();
            return ResultPattern<EmployeeConsumptionLimits>.Success(employeeComsuptionLimitEntity!, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);

        }

        // DONE: Poner en FluentValidation
        // DONE: Test this Validation. Funciona correcamente. 
        public bool DriverHasTheMethod(int driverId, int methodOfComsuptionId)
            => !_DBContext.EmployeeConsumptionLimits.Any(x => x.DriverId == driverId && x.DriverMethodOfComsuptionId == methodOfComsuptionId);
    }
}
