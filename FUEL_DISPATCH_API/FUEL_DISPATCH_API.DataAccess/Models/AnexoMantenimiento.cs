using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class AnexoMantenimiento
    {
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        public string? Ruta { get; set; }
    }
}
