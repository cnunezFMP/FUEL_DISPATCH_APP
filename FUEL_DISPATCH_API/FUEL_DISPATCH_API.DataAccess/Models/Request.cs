using FUEL_DISPATCH_API.Utils.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Request
    {
        public int Id { get; set; } // Primary Key with IDENTITY
        public int DriverId { get; set; }
        public string? Type { get; set; }   // Salida o Transferencia.
        public int VehicleId { get; set; }
        public string Status { get; set; } = ValidationConstants.PendingStatus;
        public decimal Qty { get; set; } // Quantity (decimal for precision)
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public string? CreatedBy { get; set; }
        [JsonIgnore]
        public Vehicle? Vehicle { get; set; } // Foreign Key relationship with Vehicle
        [JsonIgnore]
        public Driver? Driver { get; set; } // Foreign Key relationship with Driver
    }
}
