using FMP_DISPATCH_API.Services.Emails;
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
    public class DriversServices : GenericRepository<Driver>, IDriversServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;

        public DriversServices(FUEL_DISPATCH_DBContext dBContext, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
            : base(dBContext, httpContextAccessor)
        {
            _DBContext = dBContext;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
        }
        public override ResultPattern<Driver> Post(Driver entity)
        {
            if (CheckIfIdIsUnique(entity))
                throw new BadRequestException("Existe un conductor con esta identificacion. ");

            if (IsEmailUnique(entity))
                throw new BadRequestException("Existe un conductor con esta direccion de correo. ");

            _DBContext.Driver.Add(entity);
            _DBContext.SaveChanges();
            if (entity.Email is not null)
                _emailSender.SendEmailAsync(entity.Email,
                    AppConstants.ACCOUNT_CREATED_MESSAGE,
                    "Your account was created successfully. ");

            return base.Post(entity);
        }
        public bool CheckIfIdIsUnique(Driver entity)
        {
            string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();

            return _DBContext
                .Driver
                .Any(x => x.Identification == entity.Identification &&
                x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId));
        }
        // DONE: Chequear esta validacion. 
        public bool IsEmailUnique(Driver driver)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            return _DBContext.Driver.Any(x => x.Email == driver.Email/* &&
            x.CompanyId == int.Parse(companyId) &&
            x.BranchOfficeId == int.Parse(branchId)*/);
        }
        // DONE: Implementar esta funcion en el controlador de Driver
        public ResultPattern<List<WareHouseMovement>> GetDriverDispatches(int driverId)
        {
            /*string? companyId, branchId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();
            branchId = _httpContextAccessor.HttpContext?.Items["BranchOfficeId"]?.ToString();*/

            var driverDispatches = _DBContext.WareHouseMovement
                .AsNoTracking()
                .Where(x => /*x.CompanyId == int.Parse(companyId) &&
                x.BranchOfficeId == int.Parse(branchId) &&*/
                x.DriverId == driverId)
                .ToList();

            if (!driverDispatches.Any())
                throw new BadRequestException("This driver don't has movements registered. ");

            return ResultPattern<List<WareHouseMovement>>
                .Success
                (
                    driverDispatches,
                    StatusCodes.Status200OK,
                    "Driver dispatches obtained."
                );
        }
    }
}
