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

        public WareHouseMovementServices(FUEL_DISPATCH_DBContext dbContext,
            IHttpContextAccessor httpContextAccessor, ISAPService sapService)
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
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            if (wareHouseMovement.RequestId.HasValue)
                SetRequestForMovement(wareHouseMovement);
            if (wareHouseMovement.VehicleId.HasValue)
            {
                SetDriverIdByVehicle(wareHouseMovement);
                VehicleHasMovements(wareHouseMovement);
            }
            if (wareHouseMovement.Type is MovementsTypesEnum.Salida)
            {
                CalculateAmountForDispatch(wareHouseMovement);
            }
            // NoEnoughAmount(wareHouseMovement);
            UpdateVehicleOdometer(wareHouseMovement);
            wareHouseMovement.BranchOfficeId = int.Parse(branchId);
            _DBContext.WareHouseMovement.Add(wareHouseMovement);
            _DBContext.SaveChanges();

            // SAP POST
            //if (wareHouseMovement.Type is MovementsTypesEnum.Salida)
            //    PostSAP(wareHouseMovement).Wait();


            return ResultPattern<WareHouseMovement>.Success(wareHouseMovement, 
                StatusCodes.Status201Created, 
                "Registered dispatch. ");
        }
        #region Logic
        public bool SetDriverIdByVehicle(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var vehicleDriver = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchOfficeId))
                .FirstOrDefault(x => x.Id == wareHouseMovement.VehicleId);

            if (vehicleDriver?.DriverId is not null)
            {
                wareHouseMovement.DriverId = vehicleDriver!.DriverId;
                return true;
            }
            return false;
        }
        public bool VehicleHasMovements(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor
                        .HttpContext?
                        .Items["CompanyId"]?
                        .ToString();

            branchOfficeId = _httpContextAccessor
                            .HttpContext?
                            .Items["BranchOfficeId"]?
                            .ToString();

            var vehicleForDispatch = _DBContext.Vehicle
                .Include(x => x.WareHouseMovements)
                .AsNoTrackingWithIdentityResolution()
                .Where(x => x.CompanyId == int.Parse(companyId)
                && x.BranchOfficeId == int.Parse(branchOfficeId))
                .FirstOrDefault(v => v.Id == wareHouseMovement.VehicleId)
                ?? throw new NotFoundException("No vehicle found. ");

            //if (vehicleForDispatch!.WareHouseMovements.Any())
            //{
            //    if (CheckPreviousVehicleDispatch(wareHouseMovement))
            //        throw new BadRequestException("The odometer is equal or less than the previous dispatch.");
            //}
            return false;
        }
        public bool QtyCantBeZero(WareHouseMovement wareHouseMovement)
            => wareHouseMovement.Qty > ValidationConstants.ZeroGallons;
        public bool CheckPreviousVehicleDispatch(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var previousDispatch = _DBContext.WareHouseMovement
                .Where(x => x.VehicleId == wareHouseMovement.VehicleId &&
                x.BranchOfficeId == int.Parse(branchOfficeId))
                .OrderByDescending(x => x.WareHouseId)
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault();

            return wareHouseMovement.Odometer > previousDispatch!.Odometer;
        }
        // DONE: Corregir las funciones "CheckVehicle", "CheckDriver".
        public bool CheckVehicle(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var vehicleForDispatch = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(v => v.Id == wareHouseMovement.VehicleId &&
                v.CompanyId == int.Parse(companyId) &&
                v.BranchOfficeId == int.Parse(branchId))
                ?? throw new NotFoundException("No vehicle found. ");

            return (vehicleForDispatch.Status is not ValidationConstants.InactiveStatus
                && vehicleForDispatch!.Status is not ValidationConstants.NotAvailableStatus);
        }
        public bool CheckDriver(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            if (wareHouseMovement.Type is MovementsTypesEnum.Salida)
            {
                var driverForDispatch = _DBContext.Driver
                    .AsNoTrackingWithIdentityResolution()
                    .FirstOrDefault(d => d.Id == wareHouseMovement.DriverId &&
                                       d.BranchOfficeId == int.Parse(branchOfficeId))
                    ?? throw new NotFoundException("No driver found. ");

                return (driverForDispatch!.Status is not ValidationConstants.InactiveStatus &&
                                       driverForDispatch!.Status is not ValidationConstants.NotAvailableStatus);
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

            return branchOffice.Status is ValidationConstants.InactiveStatus;
        }
        public bool CheckDispenser(WareHouseMovement wareHouseMovement)
        {

            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            if (wareHouseMovement.DispenserId is not null && wareHouseMovement.Type is MovementsTypesEnum.Salida)
            {
                var dispenser = _DBContext.Dispenser
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(dp => dp.Id == wareHouseMovement.DispenserId &&
                dp.BranchIsland.BranchOfficeId == int.Parse(branchId) &&
                dp.BranchIsland.BranchOffice.CompanyId == int.Parse(companyId))
                ?? throw new NotFoundException("No dispenser found. ");

                return dispenser.Status is not ValidationConstants.InactiveStatus;
            }
            return true;
        }
        // DONE: Corregir la excepcion aqui
        public bool CheckWareHouseStock(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            var wareHouseStock = _DBContext.vw_ActualStock
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.WareHouseId == wareHouseMovement.WareHouseId &&
                x.ItemId == wareHouseMovement.ItemId);

            if (wareHouseStock is not null)
                return wareHouseStock!.StockQty > 0 || wareHouseStock.StockQty > wareHouseMovement.Qty;

            return false;
        }
        public bool CheckIfProductIsInTheWareHouse(WareHouseMovement wareHouseMovement)
            => _DBContext.vw_ActualStock
            .AsNoTrackingWithIdentityResolution()
            .Any(x => x.WareHouseId == wareHouseMovement!.WareHouseId && x.ItemId == wareHouseMovement!.ItemId);
        public bool CheckIfWareHousesHasActiveStatus(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            var wareHouse = (from t0 in _DBContext.WareHouse
                             join t1 in _DBContext.BranchOffices on t0.BranchOfficeId equals int.Parse(branchId)
                             join t2 in _DBContext.Companies on t0.CompanyId equals int.Parse(companyId)
                             select t0)
                            .AsNoTrackingWithIdentityResolution()
                            .FirstOrDefault()
                            ?? throw new NotFoundException("No warehouse found. ");

            var toWareHouse = _DBContext.WareHouse
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.ToWareHouseId);

            if (wareHouseMovement.Type is MovementsTypesEnum.Transferencia)
                return wareHouse!.Status is ValidationConstants.ActiveStatus &&
                    toWareHouse!.Status is ValidationConstants.ActiveStatus;

            return wareHouse!.Status is ValidationConstants.ActiveStatus;
        }
        public bool WillStockFallBelowMinimum(WareHouseMovement wareHouseMovement)
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
        }
        public bool WillStockFallMaximun(WareHouseMovement wareHouseMovement)
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
        }
        public void NoEnoughAmount(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            if (wareHouseMovement.Type is MovementsTypesEnum.Salida)
            {
                // DONE: Probar esto: (El "CompanyId" no estaba nulo. )  
                var driverCurrentAmount = (from t0 in _DBContext.EmployeeConsumptionLimits
                                           join t1 in _DBContext.Driver on t0.DriverId equals t1.Id
                                           where t0.DriverId == wareHouseMovement.DriverId &&
                                           (int)t0.DriverMethodOfComsuptionId == wareHouseMovement.FuelMethodOfComsuptionId &&
                                           t1.BranchOfficeId == int.Parse(branchOfficeId) &&
                                           t1.CompanyId == int.Parse(companyId)
                                           select t0)
                                           .AsNoTrackingWithIdentityResolution()
                                           .FirstOrDefault()
                                           ?? throw new NotFoundException("This driver don't has this method. ");


                if (driverCurrentAmount!.DriverMethodOfComsuptionId is ValidationConstants.CreditCardMethod)
                {
                    if (wareHouseMovement.Amount > driverCurrentAmount.CurrentAmount)
                        throw new BadRequestException("Driver have'nt enough amount. ");

                    var newDriverAmount = driverCurrentAmount.CurrentAmount - wareHouseMovement.Amount;
                    driverCurrentAmount.CurrentAmount = newDriverAmount;
                    _DBContext.EmployeeConsumptionLimits.Update(driverCurrentAmount);
                    _DBContext.SaveChanges();
                }

                if (driverCurrentAmount!.DriverMethodOfComsuptionId is ValidationConstants.GallonsMethod ||
                    driverCurrentAmount!.DriverMethodOfComsuptionId is ValidationConstants.TicketMethod)
                {
                    if (wareHouseMovement.Qty > driverCurrentAmount.CurrentAmount)
                        throw new BadRequestException("Driver have'nt enough amount. ");

                    var newDriverAmount = driverCurrentAmount.CurrentAmount - wareHouseMovement.Amount;
                    driverCurrentAmount.CurrentAmount = newDriverAmount;
                    _DBContext.EmployeeConsumptionLimits.Update(driverCurrentAmount);
                    _DBContext.SaveChanges();
                }
            }
        }
        // DONE: Verificar el estado de las solicitudes. Agregar a FluentValidation.
        // DONE: Hacer un movimiento con la solicitud que agregue.
        public bool SetRequestForMovement(WareHouseMovement wareHouseMovement)
        {
            string? companyId, branchOfficeId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchOfficeId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();
            // DONE: Test this:
            var requestForMovement = (from t0 in _DBContext.WareHouseMovementRequest
                                      join t1 in _DBContext.WareHouse on t0.WareHouseId equals t1.Id
                                      where t0.Id == wareHouseMovement.RequestId &&
                                      t1.BranchOfficeId == int.Parse(branchOfficeId) &&
                                      t1.CompanyId == int.Parse(companyId)
                                      select t0)
                                      .FirstOrDefault()
                                      ?? throw new NotFoundException("Request not found. ");

            if (requestForMovement.Status == ValidationConstants.PendingStatus ||
                requestForMovement.Status == ValidationConstants.RejectedStatus ||
                requestForMovement.Status == ValidationConstants.CanceledStatus)
            {
                throw new BadRequestException($"This request is {requestForMovement.Status}");
            }

            wareHouseMovement.Qty = requestForMovement.Qty;

            if (requestForMovement.DriverId.HasValue)
                wareHouseMovement.DriverId = requestForMovement.DriverId;

            if (requestForMovement.VehicleId.HasValue)
                wareHouseMovement.VehicleId = requestForMovement.VehicleId;

            requestForMovement.Status = ValidationConstants.CompletedStatus;
            _DBContext.WareHouseMovementRequest.Update(requestForMovement);
            _DBContext.SaveChanges();

            return true;
        }
        public bool CalculateAmountForDispatch(WareHouseMovement wareHouseMovement)
        {
            var articleForDispatch = _DBContext.ArticleDataMaster
                .AsNoTracking()
                .FirstOrDefault(x => x.Id == wareHouseMovement.ItemId);

            wareHouseMovement.Amount = articleForDispatch!.UnitPrice * wareHouseMovement.Qty;

            return true;
        }
        public bool UpdateVehicleOdometer(WareHouseMovement wareHouseMovement)
        {
            var vehicle = _DBContext.Vehicle
                .AsNoTrackingWithIdentityResolution()
                .FirstOrDefault(x => x.Id == wareHouseMovement.VehicleId);

            vehicle!.Odometer = wareHouseMovement.Odometer;
            _DBContext.Vehicle.Update(vehicle);
            _DBContext.SaveChanges();
            return true;
        }
        #endregion
    }
}