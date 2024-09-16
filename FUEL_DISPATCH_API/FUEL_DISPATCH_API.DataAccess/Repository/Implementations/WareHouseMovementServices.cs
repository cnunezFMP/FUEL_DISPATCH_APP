using FUEL_DISPATCH_API.DataAccess.Enums;
using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.DataAccess.Services;
using FUEL_DISPATCH_API.Utils.Constants;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class WareHouseMovementServices : GenericRepository<WareHouseMovement>, IWareHouseMovementServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISAPService sapService;
        public WareHouseMovementServices(
            FUEL_DISPATCH_DBContext dbContext,
            IHttpContextAccessor httpContextAccessor, 
            ISAPService sapService)
            : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
            this.sapService = sapService;
        }
        // DONE: Hacer despachos y transferencias con solicitudes.
        private async Task PostSAP(WareHouseMovement wareHouseMovement)
        {
            await sapService.PostGenExit(wareHouseMovement);
        }
        public override ResultPattern<WareHouseMovement> Post(WareHouseMovement wareHouseMovement)
        {
            if (!CheckIfWareHousesHasActiveStatus(wareHouseMovement))
                throw new BadRequestException("Este almacen esta inactivo. ");

            if (wareHouseMovement.RequestId.HasValue)
                SetRequestForMovement(wareHouseMovement);

            if (wareHouseMovement.VehicleId.HasValue)
                SetDriverIdByVehicle(wareHouseMovement);

            if (!QtyCantBeZero(wareHouseMovement))
                throw new BadRequestException("No se puede dispensar con cero. ");

            if (!CheckVehicleOdometer(wareHouseMovement))
                throw new BadRequestException("El odometro es menor o igual al del vehiculo. ");

            if (!CheckVehicle(wareHouseMovement))
                throw new BadRequestException("El vehiculo esta inactivo, o no esta disponible. ");

            if (!CheckDriver(wareHouseMovement))
                throw new BadRequestException("El conductor esta inactivo, o no esta disponible. ");

            /*if (!CheckBranchOffice(wareHouseMovement))
                throw new BadRequestException("El conductor esta inactivo, o no esta disponible. ");*/

            if (!CheckDispenser(wareHouseMovement))
                throw new BadRequestException("El dispensador no esta activo. ");
            // DONE: Terminar de probar esta validacion. (Simplemente no era el id del item que estaba. )
            /*if (!CheckIfProductIsInTheWareHouse(wareHouseMovement))
                throw new BadRequestException("El articulo indicado no se encuentra en el almacen. ");*/

            UpdateVehicleOdometer(wareHouseMovement);
            sapService.PostGenExit(wareHouseMovement).Wait();
            return base.Post(wareHouseMovement);
        }

        public override ResultPattern<WareHouseMovement> Update(Func<WareHouseMovement, bool> predicate, WareHouseMovement updatedEntity)
        {
            UpdateVehicleOdometer(updatedEntity);
            return base.Update(predicate, updatedEntity);
        }
        #region Logic
        public bool SetDriverIdByVehicle(WareHouseMovement wareHouseMovement)
        {
            /*string? companyId,
                    branchOfficeId;

            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();

            branchOfficeId = _httpContextAccessor
                .HttpContext?
                .Items["BranchOfficeId"]?
                .ToString();*/

            var vehicleDriver = _DBContext.Vehicle
                .FirstOrDefault(x => x.Id == wareHouseMovement.VehicleId /*&&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(bran  chOfficeId)*/) ??
                throw new NotFoundException("El vehiculo indicado no se a encontrado. ");

            if (vehicleDriver?.DriverId is not null)
            {
                wareHouseMovement.DriverId = vehicleDriver!.DriverId;
                return true;
            }
            return false;
        }
        public bool QtyCantBeZero(WareHouseMovement wareHouseMovement)
            => wareHouseMovement.Qty > ValidationConstants.ZeroGallons;
        public bool CheckVehicleOdometer(WareHouseMovement wareHouseMovement)
        {
            /*string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.VehicleId/* &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchOfficeId)*/) ??
                throw new NotFoundException("No se encontro el vehiculo para el despacho. ");

            return wareHouseMovement.Odometer > vehicleForDispatch!.Odometer;
        }
        // DONE: Corregir las funciones "CheckVehicle", "CheckDriver".
        public bool CheckVehicle(WareHouseMovement wareHouseMovement)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == wareHouseMovement.VehicleId /*&&
                v.CompanyId == int.Parse(companyId) &&
                v.BranchOfficeId == int.Parse(branchId)*/)
                ?? throw new NotFoundException("No vehicle found. ");

            return (vehicleForDispatch.Status is not VehicleStatussesEnum.Inactive
                && vehicleForDispatch!.Status is not VehicleStatussesEnum.NotAvailable);
        }
        public bool CheckDriver(WareHouseMovement wareHouseMovement)
        {
            /*string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
*/
            if (wareHouseMovement.Type is MovementsTypesEnum.Salida)
            {
                var driverForDispatch = _DBContext.Driver
                    .AsNoTrackingWithIdentityResolution()
                    .FirstOrDefault(d => d.Id == wareHouseMovement.DriverId /*&&
                                       d.BranchOfficeId == int.Parse(branchOfficeId)*/)
                    ?? throw new NotFoundException("No driver found. ");

                return (driverForDispatch!.Status is not ActiveInactiveStatussesEnum.Inactive);
            }

            return true;
        }
        public bool CheckBranchOffice(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            var branchOffice = _DBContext.BranchOffices
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(b => b.Id == int.Parse(branchId) &&
                b.CompanyId == int.Parse(companyId))
                ?? throw new NotFoundException("No branch office found. ");

            return branchOffice.Status is not ValidationConstants.InactiveStatus;
        }
        public bool CheckDispenser(WareHouseMovement wareHouseMovement)
        {
            /* string? companyId, branchId;
             companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
             branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/


            var dispenser = _DBContext.Dispenser
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefault(dp => dp.Id == wareHouseMovement.DispenserId /*&&
            dp.BranchOfficeId == int.Parse(branchId) &&
            dp.CompanyId == int.Parse(companyId)*/)
            ?? throw new NotFoundException("No se encontro el dispensador. ");

            return dispenser.Status is not ActiveInactiveStatussesEnum.Inactive;

        }
        // DONE: Corregir la excepcion aqui
        public bool CheckWareHouseStock(WareHouseMovement wareHouseMovement)
        {
            /*string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId &&
                x.ItemId == wareHouseMovement.ItemId)
                ?? throw new NotFoundException("No existe relacion del almacen con el articulo. ");

            if (wareHouseStock is not null)
                return wareHouseStock!.StockQty > 0 || wareHouseStock.StockQty > wareHouseMovement.Qty;

            return false;
        }
        public bool CheckIfProductIsInTheWareHouse(WareHouseMovement wareHouseMovement)
            => _DBContext.vw_ActualStock
            .AsNoTrackingWithIdentityResolution()
            .Any(x => x.WareHouseId == wareHouseMovement!.WareHouseId &&
            x.ItemId == wareHouseMovement!.ItemId);
        public bool CheckIfWareHousesHasActiveStatus(WareHouseMovement wareHouseMovement)
        {
            /* string? companyId, branchId;
             companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
             branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/
            var wareHouse = _DBContext.WareHouse
                            .AsNoTrackingWithIdentityResolution()
                            .FirstOrDefault(x => x.Id == wareHouseMovement.WareHouseId)
                            ?? throw new NotFoundException("No warehouse found. ");


            return wareHouse!.Status is not ActiveInactiveStatussesEnum.Inactive;
        }
        /*public bool WillStockFallBelowMinimum(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId &&
                x.BranchOfficeId == int.Parse(branchOfficeId) &&
                x.CompanyId == int.Parse(companyId));

            var wareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.WareHouseId &&
                x.BranchOfficeId == int.Parse(branchOfficeId) &&
                x.CompanyId == int.Parse(companyId))
                ?? throw new NotFoundException("No warehouse found. ");

            if (wareHouseStock is not null)
            {
                decimal currentQtyInWareHouse = wareHouseStock!.StockQty - wareHouseMovement.Qty;
                return currentQtyInWareHouse < wareHouse!.MinCapacity;
            }
            return false;
        }*/
        /*public bool WillStockFallMaximun(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            if (wareHouseMovement.Type is MovementsTypesEnum.Transferencia)
            {

                var toWareHouseStock = _DBContext.vw_ActualStock
                    .AsNoTrackingWithIdentityResolution()
                    .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.ToWareHouseId &&
                     x.BranchOfficeId == int.Parse(branchOfficeId) &&
                     x.CompanyId == int.Parse(companyId));

                var toWareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.ToWareHouseId &&
                 x.BranchOfficeId == int.Parse(branchOfficeId) &&
                 x.CompanyId == int.Parse(companyId));

                if (toWareHouseStock is not null)
                {
                    decimal currentQtyInToWareHouse = toWareHouseStock!.StockQty + wareHouseMovement.Qty;
                    return currentQtyInToWareHouse > toWareHouse!.MaxCapacity;
                }

            }

            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId &&
                x.BranchOfficeId == int.Parse(branchOfficeId) &&
                x.CompanyId == int.Parse(companyId));


            var wareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.WareHouseId &&
                x.BranchOfficeId == int.Parse(branchOfficeId) &&
                x.CompanyId == int.Parse(companyId));

            if (wareHouseStock is not null)
            {
                decimal currentQtyInWareHouse = wareHouseStock!.StockQty + wareHouseMovement.Qty;
                return currentQtyInWareHouse > wareHouse!.MaxCapacity;
            }
            return false;
        }*/
        /*public void NoEnoughAmount(WareHouseMovement wareHouseMovement)
        {
            string? companyId,
                    branchOfficeId;

            companyId = _httpContextAccessor
                        .HttpContext?
                        .Items["CompanyId"]?
                        .ToString();
            branchOfficeId = _httpContextAccessor.
                             HttpContext?.
                             Items["BranchOfficeId"]?
                             .ToString();

            var driverCurrentAmount = _DBContext
            .EmployeeConsumptionLimits
            .AsNoTrackingWithIdentityResolution()
            .FirstOrDefault(x => x.CompanyId == int.Parse(companyId) &&
            x.BranchOfficeId == int.Parse(branchOfficeId) &&
            x.DriverId == wareHouseMovement.DriverId &&
            x.DriverMethodOfComsuptionId == wareHouseMovement.FuelMethodOfComsuptionId)
            ?? throw new NotFoundException("This driver don't has this method. ");

            if (driverCurrentAmount!.DriverMethodOfComsuptionId is ValidationConstants.CreditCardMethod)
            {
                if (wareHouseMovement.Amount > driverCurrentAmount.LimitAmount)
                    throw new BadRequestException("Driver have'nt enough amount. ");
                // Si el current amounrt no puede pasar el liminte. El current amount debe empezar en cero.. 
                var newDriverAmount = driverCurrentAmount.CurrentAmount + wareHouseMovement.Amount;
                driverCurrentAmount.CurrentAmount = newDriverAmount;
                _DBContext.EmployeeConsumptionLimits.Update(driverCurrentAmount);
                _DBContext.SaveChanges();
            }

            if (driverCurrentAmount!.DriverMethodOfComsuptionId is ValidationConstants.GallonsMethod ||
                driverCurrentAmount!.DriverMethodOfComsuptionId is ValidationConstants.TicketMethod)
            {
                if (wareHouseMovement.Qty > driverCurrentAmount.LimitAmount)
                    throw new BadRequestException("Driver have'nt enough amount. ");
                // El current amounrt no puede pasar el liminte. El current amount debe empezar en cero.. 
                var newDriverAmount = driverCurrentAmount.CurrentAmount + wareHouseMovement.Amount;
                driverCurrentAmount.CurrentAmount = newDriverAmount;
                _DBContext.EmployeeConsumptionLimits.Update(driverCurrentAmount);
                _DBContext.SaveChanges();

            }
        }*/
        // DONE: Verificar el estado de las solicitudes. Agregar a FluentValidation.
        // DONE: Hacer un movimiento con la solicitud que agregue.
        public bool SetRequestForMovement(WareHouseMovement wareHouseMovement)
        {
            /* string? companyId, branchOfficeId;
             companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
             branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/
            // DONE: Test this:
            var requestForMovement = _DBContext
                                      .WareHouseMovementRequest
                                      .FirstOrDefault(x => x.Id == wareHouseMovement.RequestId /*&&
                                                      x.CompanyId == int.Parse(companyId) &&
                                                      x.BranchOfficeId == int.Parse(branchOfficeId*/)
                                      ?? throw new NotFoundException("Request not found. ");

            if (requestForMovement.Status == RequestStatussesEnum.Rejected ||
                requestForMovement.Status == RequestStatussesEnum.Canceled)
                throw new BadRequestException($"Esta solicitud esta {requestForMovement.Status}");


            wareHouseMovement.Qty = requestForMovement.Qty;

            if (requestForMovement.DriverId.HasValue)
                wareHouseMovement.DriverId = requestForMovement.DriverId;

            if (requestForMovement.VehicleId.HasValue)
                wareHouseMovement.VehicleId = requestForMovement.VehicleId;

            requestForMovement.Status = RequestStatussesEnum.Completed;
            _DBContext.WareHouseMovementRequest.Update(requestForMovement);
            _DBContext.SaveChanges();

            return true;
        }
        /*public bool CalculateAmountForDispatch(WareHouseMovement wareHouseMovement)
        {
            var articleForDispatch = _DBContext.ArticleDataMaster
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == wareHouseMovement.ItemId) ??
                throw new NotFoundException("Article not found. ");

            wareHouseMovement.Amount = articleForDispatch!.UnitPrice * wareHouseMovement.Qty;

            return true;
        }*/
        public bool UpdateVehicleOdometer(WareHouseMovement wareHouseMovement)
        {
            var vehicle = _DBContext.Vehicle
                .FirstOrDefault(x => x.Id == wareHouseMovement.VehicleId)
                ?? throw new NotFoundException("No vehicle found. ");

            vehicle!.Odometer = wareHouseMovement.Odometer;
            _DBContext.Vehicle.Update(vehicle);
            _DBContext.SaveChanges();
            return true;
        }
        #endregion
    }
}