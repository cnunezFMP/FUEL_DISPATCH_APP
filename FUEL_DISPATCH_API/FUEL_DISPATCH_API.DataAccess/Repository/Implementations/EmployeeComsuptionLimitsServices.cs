using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class EmployeeComsuptionLimitsServices : GenericRepository<EmployeeConsumptionLimits>, IEmployeeComsuptionLimitsServices
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

            var method = _DBContext.EmployeeConsumptionLimits.Find(driverId, methodId)
                ?? throw new NotFoundException("This method doesn't exist. ");
            _DBContext.EmployeeConsumptionLimits.Remove(method!);
            _DBContext.SaveChanges();
            return ResultPattern<Driver>.Success(driver, StatusCodes.Status200OK, "Method updated. ");
        }
        // TODO: Actualizar.
        public ResultPattern<Driver> UpdateDriverMethod(int userId, int roleId, EmployeeConsumptionLimits employeeConsumptionLimit)
        {
            var driver = _DBContext.Driver
                .Include(x => x.DriverMethodsOfComsuption)
                .FirstOrDefault(x => x.Id == userId)
                ?? throw new NotFoundException("This driver doesn't exist. ");

            var method = _DBContext.DriverMethodOfComsuption.Find(roleId)
                ?? throw new NotFoundException("This method doesn't exist. ");

            driver.DriverMethodsOfComsuption!.Add(method);
            _DBContext.Driver.Update(driver);
            _DBContext.EmployeeConsumptionLimits.Update(employeeConsumptionLimit);
            _DBContext.SaveChanges();
            return ResultPattern<Driver>.Success(driver, StatusCodes.Status200OK, "User rols updated. ");
        }

        // DONE: Poner en FluentValidation
        // TODO: Test this Validation.
        public bool DriverHasTheMethod(int driverId, int methodOfComsuptionId)
            => !_DBContext.EmployeeConsumptionLimits.Any(x => x.DriverId == driverId && x.DriverMethodOfComsuptionId == methodOfComsuptionId);
    }
}
