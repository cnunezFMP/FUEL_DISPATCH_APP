using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class EmployeeComsuptionLimitsServices : GenericRepository<EmployeeConsumptionLimits>, IEmployeeComsuptionLimits
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public EmployeeComsuptionLimitsServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }
        public ResultPattern<Driver> DeleteDriverMethod(int driverId, int methodId)
        {
            var driver = _DBContext.Driver.Include(x => x.DriverMethodsOfComsuption).FirstOrDefault(x => x.Id == driverId)
                ?? throw new NotFoundException("This user doesn't exist. ");

            var method = _DBContext.EmployeeConsumptionLimit.Find(driverId, methodId)
                ?? throw new NotFoundException("This method doesn't exist. ");
            _DBContext.EmployeeConsumptionLimit.Remove(method!);
            _DBContext.SaveChanges();
            return ResultPattern<Driver>.Success(driver, StatusCodes.Status200OK, "Method updated. ");
        }
        public ResultPattern<Driver> UpdateDriverMethod(int driverId, int methodId)
        {

            var driverMethod = _DBContext.EmployeeConsumptionLimit.FirstOrDefault(x => x.DriverId == driverId && x.MethodOfComsuptionId == methodId);

            var driver = _DBContext.Driver.Include(x => x.DriverMethodsOfComsuption).FirstOrDefault(x => x.Id == driverId)
                ?? throw new NotFoundException("This user doesn't exist. ");

            var method = _DBContext.DriverMethodOfComsuption.Find(methodId)
                ?? throw new NotFoundException("This method doesn't exist. "); ;

            driver.DriverMethodsOfComsuption.Add(method);
            _DBContext.Driver.Update(driver);
            _DBContext.SaveChanges();
            return ResultPattern<Driver>.Success(driver, StatusCodes.Status200OK, "Driver method updated. ");
        }
        // TODO: Test this Validation.
        // DONE: Poner en FluentValidation
        public bool DriverHasTheMethod(Driver driver, DriverMethodOfComsuption driverMethodOfComsuption)
            => !driver.DriverMethodsOfComsuption.Any(r => r.Id == driverMethodOfComsuption.Id);
    }
}
