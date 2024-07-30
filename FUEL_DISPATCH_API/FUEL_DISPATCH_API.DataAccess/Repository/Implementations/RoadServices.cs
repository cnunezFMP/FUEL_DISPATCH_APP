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
    public class RoadServices : GenericRepository<Road>, IRoadServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public RoadServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor) 
        { 
            _DBContext = dbContext; 
        }
        // Indica que hay alguna ruta con el mismo codigo.
        public bool RoadCodeMustBeUnique(Road road)
            => _DBContext.Road.Any(x => x.Code == road.Code);
        
    }
}
