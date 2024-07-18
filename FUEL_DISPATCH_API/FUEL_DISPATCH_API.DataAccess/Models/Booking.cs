using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Booking
    {
        public int Id { get; set; }
        [Required] public int VehicleId { get; set; }
        [Required] public int DriverId { get; set; }
        [Required] public DateTime SpecificDate { get; set; }
        public DateTime? ToSpecificDate { get; set; }
        [Required] public DateTime? PickUpDate { get; set; }
        public string? Status { get; set; }
        public string? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; } = DateTime.Now;

        public string? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public string? Notes { get; set; }

        [JsonIgnore] public virtual Vehicle? Vehicle { get; set; }

        [JsonIgnore] public virtual Driver? Driver { get; set; }
    }
}
