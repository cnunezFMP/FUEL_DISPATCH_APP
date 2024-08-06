using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models.SAP
{
    public class LoginResponse
    {
        public string? OdataMetadata { get; set; }
        public string? SessionId { get; set; }
        public string? Version { get; set; }
        public int SessionTimeout { get; set; }
    }

}
