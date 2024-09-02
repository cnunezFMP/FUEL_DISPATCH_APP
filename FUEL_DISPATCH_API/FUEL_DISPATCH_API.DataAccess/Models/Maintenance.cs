using FUEL_DISPATCH_API.DataAccess.Enums;
using FUEL_DISPATCH_API.Utils.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Maintenance
    {
        public int? Id { get; set; }
        [Required] public int? VehicleId { get; set; }
        [Required] public int? PartId { get; set; }
        public int? CompanyId { get; set; }
        public int? BranchOfficeId { get; set; }
        public decimal? CurrentOdometer { get; set; } // Se asignara con el odometer del vehiculo
        [Required] public string? Code { get; set; }
        public decimal? OdometerUpcomingMaintenance { get; set; }
        public MaitenanceStatusEnum? Status { get; set; } = MaitenanceStatusEnum.NotStarted;
        public DateTime? NextMaintenanceDate { get; set; }
        public string? Technician { get; set; }
        public string? VehicleVin { get; set; }
        public string? PartCode { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore] public virtual Vehicle? Vehicle { get; set; }
        [JsonIgnore] public virtual Part? Part { get; set; }
    }

}
