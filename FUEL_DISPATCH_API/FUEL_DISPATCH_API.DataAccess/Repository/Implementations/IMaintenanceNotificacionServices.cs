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
    public interface IMaintenanceNotificacionServices : IGenericInterface<MaitenanceNotification>
    {
    }

    public class MaintenanceNotificacionServices(FUEL_DISPATCH_DBContext DbContext, IHttpContextAccessor httpContextAccessor) :
        GenericRepository<MaitenanceNotification>(DbContext, httpContextAccessor), IMaintenanceNotificacionServices
    {

    }

}
