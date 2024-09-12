using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ComsuptionByMonthServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<ComsuptionByMonth>(dbContext, httpContextAccessor), IComsuptionByMonthServices
    {
    }
}
