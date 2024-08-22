using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class LicenseExpDateAlertServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<vw_LicenseExpDateAlert>(DBContext, httpContextAccessor), ILicenseExpDateAlertServices
    {
    }
}
