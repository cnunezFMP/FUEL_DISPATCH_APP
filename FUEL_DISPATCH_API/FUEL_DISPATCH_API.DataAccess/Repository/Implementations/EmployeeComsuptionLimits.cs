using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class EmployeeComsuptionLimits : GenericRepository<EmployeeConsumptionLimits>, IEmployeeComsuptionLimits
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public EmployeeComsuptionLimits(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }
        public ResultPattern<EmployeeConsumptionLimits> DeleteDriverMethod(int driverId, int methodId)
        {
            var driver = _DBContext.User.Include(x => x.Rols).FirstOrDefault(x => x.Id == driverId)
                ?? throw new NotFoundException("This user doesn't exist. ");

            var method = _DBContext.EmployeeConsumptionLimit.Find(driverId, methodId)
                ?? throw new NotFoundException("This method doesn't exist. ");

            _DBContext.EmployeeConsumptionLimit.Remove(method!);
            _DBContext.SaveChanges();
            return ResultPattern<EmployeeConsumptionLimits>.Success(driver, StatusCodes.Status200OK, "Method updated. ");
        }
        /*public ResultPattern<EmployeeConsumptionLimits> UpdateDriverMethod(int driverId, int methodId)
        {
            var driver = _DBContext.Driver.Include(x => x.DriverMethodsOfComsuption).FirstOrDefault(x => x.Id == driverId);
            if (driver is null)
                throw new NotFoundException("This user doesn't exist. ");

            var method = _DBContext.DriverMethodOfComsuption.Find(methodId);

            if (method is null)
                throw new NotFoundException("This method doesn't exist. ");

            if (DriverHasTheMethod(driver, methodId))
                throw new BadRequestException("This driver has this method. ");
            driver.DriverMethodsOfComsuption.Add(method);
            _DBContext.Driver.Update(driver);
            _DBContext.SaveChanges();
            return ResultPattern<EmployeeConsumptionLimits>.Success(driver, StatusCodes.Status200OK, "Driver method updated. ");
        }*/
        // TODO: Poner en fluentvalidation
        public bool DriverHasTheMethod(Driver driver, int methodId)
            => driver.DriverMethodsOfComsuption.Any(r => r.Id == methodId);
    }
}
