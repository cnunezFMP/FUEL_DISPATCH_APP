using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class OdometerMeasureServices : GenericRepository<OdometerMeasure>, IOdometerMeasureServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public OdometerMeasureServices(FUEL_DISPATCH_DBContext dbContext)
            : base(dbContext)
        {
            _DBContext = dbContext;
        }
    }

    public interface IOdometerMeasureServices : IGenericInterface<OdometerMeasure>
    {
    }
}
