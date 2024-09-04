using FUEL_DISPATCH_API.DataAccess.Models;
using FUEL_DISPATCH_API.DataAccess.Repository.GenericRepository;
using FUEL_DISPATCH_API.DataAccess.Repository.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Repository.Implementations
{
    public class PermissionsServices(FUEL_DISPATCH_DBContext DBContext, IHttpContextAccessor httpContextAccessor) :
        GenericRepository<Permission>(DBContext, httpContextAccessor), IPermissionsServices
    {
    }
}
