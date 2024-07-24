using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class DriverMethodOfComsuptionServices : GenericRepository<Models.DriverMethodOfComsuption>, IDriverMethodOfComsuptionServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public DriverMethodOfComsuptionServices(FUEL_DISPATCH_DBContext dbContext) : base(dbContext) { _DBContext = dbContext; }
    }
}
