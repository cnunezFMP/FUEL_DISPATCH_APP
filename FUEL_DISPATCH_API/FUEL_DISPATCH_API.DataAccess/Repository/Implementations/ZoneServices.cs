using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ZoneServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _DBContext = dbContext;
        }

        public override ResultPattern<Zone> Post(Zone entity)
        {
            if (ZoneCodeMustBeUnique(entity))
                throw new BadRequestException("Existe una zona registrada con este codigo. ");
            return base.Post(entity);
        }

        public bool ZoneCodeMustBeUnique(Zone zone)
        {
           /* string? companyId;
            companyId = _httpContextAccessor.HttpContext?.Items["CompanyId"]?.ToString();*/

            return _DBContext
                .Zone
                .Any(x => x.Code == zone.Code /*&&
                x.CompanyId == int.Parse(companyId)*/);
        }
    }
}
