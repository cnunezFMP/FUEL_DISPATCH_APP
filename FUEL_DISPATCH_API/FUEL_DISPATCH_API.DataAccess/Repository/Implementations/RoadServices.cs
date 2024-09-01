using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using FUEL_DISPATCH_API.Utils.Exceptions;
using FUEL_DISPATCH_API.Utils.ResponseObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class RoadServices : GenericRepository<Road>, IRoadServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RoadServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
        {
            _DBContext = dbContext;
        }
        // Indica que hay alguna ruta con el mismo codigo.
        public override ResultPattern<Road> Post(Road entity)
        {
            if (RoadCodeMustBeUnique(entity))
                throw new BadRequestException("Existe una ruta con el mismo codigo asignado. ");


            return base.Post(entity);
        }
        public bool RoadCodeMustBeUnique(Road road)
        {
            string? companyId;
            companyId = _httpContextAccessor
                .HttpContext?
                .Items["CompanyId"]?
                .ToString();

            return _DBContext.Road
                .AsNoTracking()
                .Where(x => x.CompanyId == int.Parse(companyId))
                .Any();
        }
    }
}
