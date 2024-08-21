using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ActualStockServices : GenericRepository<vw_ActualStock>, IActualStockServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ActualStockServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
