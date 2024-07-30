using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ZoneServices : GenericRepository<Zone>, IZoneServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public ZoneServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }

        public bool ZoneCodeMustBeUnique(Zone zone)
        {
            return !_DBContext.Zone.Any(x => x.Code == zone.Code);
        }
    }
}
