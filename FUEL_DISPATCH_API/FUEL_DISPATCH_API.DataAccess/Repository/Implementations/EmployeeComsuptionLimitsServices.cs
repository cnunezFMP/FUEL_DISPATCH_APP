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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public EmployeeComsuptionLimitsServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(
                    dbContext,
                    httpContextAccessor
                  )
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public override ResultPattern<EmployeeConsumptionLimits> Post(EmployeeConsumptionLimits entity)
        {
            if (DriverHasTheMethod(entity.DriverId, entity.DriverMethodOfComsuptionId))
                throw new BadRequestException("Este conductor ya tiene el metodo asignado. ");
            return base.Post(entity);
        }
        public override ResultPattern<EmployeeConsumptionLimits> Delete(Func<EmployeeConsumptionLimits, bool> predicate)
        {
            var employeeComsuptionLimitEntityToDelete = _DBContext
                .EmployeeConsumptionLimits
                .FirstOrDefault(predicate);

            _DBContext.EmployeeConsumptionLimits.Remove(employeeComsuptionLimitEntityToDelete!);
            _DBContext.SaveChanges();
            return ResultPattern<EmployeeConsumptionLimits>.Success(employeeComsuptionLimitEntityToDelete!,
                                                                    StatusCodes.Status200OK,
                                                                    "Driver method deleted. ");
        }
        public ResultPattern<EmployeeConsumptionLimits> Update
            (
            int driverId,
            int methodId,
            EmployeeConsumptionLimits updatedEntity
            )
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor
                        .HttpContext?
                        .Items["CompanyId"]?
                        .ToString();

            branchId = _httpContextAccessor
                       .HttpContext?
                       .Items["BranchOfficeId"]?
                       .ToString();

            var employeeComsuptionLimitEntity = _DBContext.EmployeeConsumptionLimits
                .Include(x => x.Driver)
                .FirstOrDefault(x => x.DriverId == driverId &&
                x.DriverMethodOfComsuptionId == methodId &&
                x.CompanyId == updatedEntity.Driver!.CompanyId &&
                x.BranchOfficeId == updatedEntity.Driver.BranchOfficeId) ??
                throw new NotFoundException("No relation found. ");

            _DBContext
                .Entry(employeeComsuptionLimitEntity!)
                .CurrentValues
                .SetValues(updatedEntity);

            _DBContext.Update(employeeComsuptionLimitEntity);
            _DBContext.SaveChanges();
            return ResultPattern<EmployeeConsumptionLimits>.Success(employeeComsuptionLimitEntity!, StatusCodes.Status200OK, AppConstants.DATA_UPDATED_MESSAGE);
        }

        public bool DriverHasTheMethod(int driverId, int methodOfComsuptionId)
            => _DBContext.EmployeeConsumptionLimits.Any(x => x.DriverId == driverId && (int)x.DriverMethodOfComsuptionId == methodOfComsuptionId);

    }
}
