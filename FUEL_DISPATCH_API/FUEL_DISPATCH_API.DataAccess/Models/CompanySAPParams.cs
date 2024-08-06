using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class CompanySAPParams
    {
        public int CompanyId { get; set; }
        public required string ServiceLayerURL { get; set; }
        public string? CompanyDB { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
