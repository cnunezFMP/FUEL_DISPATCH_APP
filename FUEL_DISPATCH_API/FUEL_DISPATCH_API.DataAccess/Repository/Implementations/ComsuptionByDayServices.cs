using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ComsuptionByDayServices : GenericRepository<ComsuptionByDay>, IComsuptionByDayServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public ComsuptionByDayServices(FUEL_DISPATCH_DBContext dbContext) : base(dbContext) { _DBContext = dbContext; }
    }
}
