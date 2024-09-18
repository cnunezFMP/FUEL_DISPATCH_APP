using FUEL_DISPATCH_API.DataAccess.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class MaintenanceDetails
    {
        public int Id { get; set; }
        public int MaintenanceId { get; set; }
        [Required] public int PartId { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? PartCode { get; set; }
        public string? Notes { get; set; }
        public decimal? OdometerUpcomingMaintenance { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        //[JsonIgnore] public Part? Part { get; set; }
    }
}
