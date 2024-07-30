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
            if (CheckDispatch(entity))
                throw new BadRequestException("Revise que el odometro registrado no sea igual o menor al anterior." +
                                              "Tambien, que la cantidad de combustible digitados no esten en cero. ");
            if (CheckVehicle(entity))
                throw new BadRequestException("Puede que el vehiculo no exista, o este inactivo. ");

            _DBContext.WareHouseMovementRequest.Add(entity);
            _DBContext.SaveChanges();
            return ResultPattern<WareHouseMovementRequest>.Success(entity, StatusCodes.Status201Created, "Solicitud enviada. ");
        }
        public bool CheckDispatch(WareHouseMovementRequest newRequest)
            => newRequest.Qty is ValidationConstants.ZeroGallons;
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
            var vehicleForDispatch = _DBContext.Vehicle.FirstOrDefault(x => x.Id == newRequest.VehicleId);
            return vehicleForDispatch!.Status is not ValidationConstants.InactiveStatus || vehicleForDispatch!.Status is not ValidationConstants.NotAvailableStatus;
        }
    }
}
