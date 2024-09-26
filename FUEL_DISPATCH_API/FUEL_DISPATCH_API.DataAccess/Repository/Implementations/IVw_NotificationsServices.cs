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
    public interface IVw_NotificationsServices : IGenericInterface<Vw_Notifications>
    {
    }

    public class Vw_NotificationsServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) : GenericRepository<Vw_Notifications>(DBContext, httpContextAccessor), IVw_NotificationsServices
    {
        private readonly FUEL_DISPATCH_DBContext _DBContext = DBContext;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    }
}
