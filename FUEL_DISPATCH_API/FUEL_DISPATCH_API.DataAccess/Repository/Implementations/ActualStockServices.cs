using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ActualStockServices : GenericRepository<vw_ActualStock>, IActualStockServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public ActualStockServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }
    }
}
