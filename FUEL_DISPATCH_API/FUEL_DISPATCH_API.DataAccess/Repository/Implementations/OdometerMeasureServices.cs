using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class OdometerMeasureServices : GenericRepository<OdometerMeasure>, IOdometerMeasureServices
    {
        public OdometerMeasureServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor)
            : base(dbContext, httpContextAccessor)
        {
        }
    }

    public interface IOdometerMeasureServices : IGenericInterface<OdometerMeasure>
    {
    }
}
