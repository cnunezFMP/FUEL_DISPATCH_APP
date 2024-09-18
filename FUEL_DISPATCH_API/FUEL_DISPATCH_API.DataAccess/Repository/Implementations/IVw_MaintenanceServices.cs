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
    public interface IVw_MaintenanceServices : IGenericInterface<vw_Maintenance>
    {
    }

    public class Vw_MaintenanceServices(FUEL_DISPATCH_DBContext dbContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<vw_Maintenance>(dbContext, httpContextAccessor), IVw_MaintenanceServices
    {

    }
}
