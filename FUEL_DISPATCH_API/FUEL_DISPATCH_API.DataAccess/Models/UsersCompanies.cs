using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable disable
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class UsersCompanies
    {
        public int UserId { get; set; }
        public int CompanyId { get; set; }
        public virtual User User { get; set; }
        public virtual Companies Company { get; set; }
    }
}
