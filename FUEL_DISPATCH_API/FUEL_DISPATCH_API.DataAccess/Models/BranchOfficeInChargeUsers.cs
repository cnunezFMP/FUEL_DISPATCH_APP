using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class BranchOfficeInChargeUsers
    {
        public int UserId { get; set; }
        public int BranchOfficeId { get; set; }
        public User? User { get; set; }
        public BranchOffices? BranchOffice { get; set; }

    }
}
