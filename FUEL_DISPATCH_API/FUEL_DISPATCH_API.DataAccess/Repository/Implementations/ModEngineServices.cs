using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class ModEngineServices : GenericRepository<ModEngine>, IModEngineServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext;
        public ModEngineServices(FUEL_DISPATCH_DBContext dbContext) 
            : base(dbContext) 
        { 
            _DBContext = dbContext; 
        }
    }
}
