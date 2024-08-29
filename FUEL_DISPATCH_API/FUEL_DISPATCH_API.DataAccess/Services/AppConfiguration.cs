using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Services
{
    public static class AppConfiguration
    {
        public static IConfiguration? Configuration { get; set; }

    }
}
