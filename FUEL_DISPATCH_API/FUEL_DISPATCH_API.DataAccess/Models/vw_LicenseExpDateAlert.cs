using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class vw_LicenseExpDateAlert
    {
        public string? FullName { get; set; }
        public DateTime? LicenceExpDate { get; set; }
        public int? CompanyId { get; set; }
    }
}
