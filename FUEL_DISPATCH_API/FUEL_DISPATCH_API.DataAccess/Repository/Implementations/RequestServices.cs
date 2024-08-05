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
    public class RequestServices : GenericRepository<WareHouseMovementRequest>, IRequestServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public RequestServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor)
            : base(DBContext, httpContextAccessor)
        {
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
            var wareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovementRequest.WareHouseId);

            var toWareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovementRequest.ToWareHouseId);

            if (wareHouseMovementRequest.Type is MovementsTypesEnum.Transferencia)
                return wareHouse!.Status is not ValidationConstants.InactiveStatus
                    && toWareHouse!.Status is not ValidationConstants.InactiveStatus;

            return wareHouse!.Status is not ValidationConstants.InactiveStatus;
        }
        public bool CheckVehicle(WareHouseMovementRequest newRequest)
        {
            var vehicleForDispatch = _DBContext.Vehicle
                .FirstOrDefault(x => x.Id == newRequest.VehicleId)
                ?? throw new BadRequestException("This vehicle doesn't exist. ");

            return vehicleForDispatch!.Status is not
                ValidationConstants.InactiveStatus &&
                vehicleForDispatch!.Status is not ValidationConstants.NotAvailableStatus;
        }

        public bool CheckDriver(WareHouseMovementRequest newRequest)
        {
            var driverForDispatch = _DBContext.Driver
                .FirstOrDefault(x => x.Id == newRequest.DriverId)
                ?? throw new NotFoundException("This driver doesn't exist. ");

            return driverForDispatch!.Status is not
                ValidationConstants.InactiveStatus &&
                driverForDispatch!.Status is not ValidationConstants.NotAvailableStatus;
        }
    }
}
