using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class GenerationServices : GenericRepository<Generation>, IGenerationServices
    {
        public GenerationServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
        }
    }
}
