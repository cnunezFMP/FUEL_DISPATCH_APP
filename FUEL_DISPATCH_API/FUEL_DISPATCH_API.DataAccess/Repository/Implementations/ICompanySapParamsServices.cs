using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public interface ICompanySapParamsServices : IGenericInterface<CompanySAPParams>
    {
    }

    public class CompanySapParamsServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<CompanySAPParams>(DBContext, httpContextAccessor), ICompanySapParamsServices 
    {
    
    }
}
