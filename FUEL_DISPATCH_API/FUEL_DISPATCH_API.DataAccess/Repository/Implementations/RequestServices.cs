using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;


namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class RequestServices : GenericRepository<WareHouseMovementRequest>, IRequestServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public RequestServices(FUEL_DISPATCH_DBContext DBContext)
            : base(DBContext)
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

        public bool CheckVehicle(WareHouseMovementRequest newRequest)
        {
            var vehicleForDispatch = _DBContext.Vehicle.FirstOrDefault(x => x.Id == newRequest.VehicleId);
            return vehicleForDispatch!.Status is not ValidationConstants.InactiveStatus || vehicleForDispatch!.Status is not ValidationConstants.NotAvailableStatus;
        }
    }
}
