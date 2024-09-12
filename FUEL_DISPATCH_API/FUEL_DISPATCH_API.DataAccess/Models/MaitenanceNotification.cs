using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class MaitenanceNotification
    {
        public int Id { get; set; }
        public string? Ficha { get; set; }
        public decimal? Odometer { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public decimal? OdometerUpcomingMaintenance { get; set; }
    }
}
