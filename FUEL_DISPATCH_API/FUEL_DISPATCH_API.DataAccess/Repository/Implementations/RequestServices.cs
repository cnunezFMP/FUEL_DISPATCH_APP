using FUEL_DISPATCH_API.DataAccess.Enums;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;


namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class RequestServices : GenericRepository<WareHouseMovementRequest>, IRequestServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RequestServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor)
            : base(DBContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = DBContext;
        }
        public override ResultPattern<WareHouseMovementRequest> Post(WareHouseMovementRequest entity)
        {
            _DBContext.WareHouseMovementRequest.Add(entity);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouseMovementRequest>.Success(entity, StatusCodes.Status201Created, "Solicitud enviada. ");
        }
        public bool CheckIfWareHousesHasActiveStatus(WareHouseMovementRequest wareHouseMovementRequest)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            var wareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovementRequest.WareHouseId &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId))
                ?? throw new NotFoundException("No warehouse found. ");

            var toWareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovementRequest.ToWareHouseId &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId))
                ?? throw new NotFoundException("No destination warehouse found. ");

            if (wareHouseMovementRequest.Type is MovementsTypesEnum.Transferencia)
                return wareHouse!.Status is not ValidationConstants.InactiveStatus
                    && toWareHouse!.Status is not ValidationConstants.InactiveStatus;

            return wareHouse!.Status is not ValidationConstants.InactiveStatus;
        }
        public bool CheckVehicle(WareHouseMovementRequest newRequest)
        {
            string? companyId, branchOfficeId;
            // DONE: Utilizar httpContextAccessor para obtener el companyId y branchOfficeId.
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == newRequest.VehicleId &&
                v.CompanyId == int.Parse(companyId) &&
                v.BranchOfficeId == int.Parse(branchOfficeId))
                ?? throw new NotFoundException("No vehicle found. ");

            return (vehicleForDispatch.Status is not VehicleStatussesEnum.Inactive
                && vehicleForDispatch!.Status is not VehicleStatussesEnum.NotAvailable);
        }
        public bool CheckDriver(WareHouseMovementRequest newRequest)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var driverForDispatch = _DBContext.Driver
                .FirstOrDefault(x => x.Id == newRequest.DriverId &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId))
                ?? throw new NotFoundException("This driver doesn't exist. ");

            return (driverForDispatch!.Status is not ValidationConstants.InactiveStatus &&
                driverForDispatch!.Status is not ValidationConstants.NotAvailableStatus);
        }
    }
}
