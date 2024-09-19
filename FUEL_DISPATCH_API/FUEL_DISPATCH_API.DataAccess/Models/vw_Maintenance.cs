using FUEL_DISPATCH_API.DataAccess.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class vw_Maintenance
    {
        public int? Id { get; set; }
        public int? DetailId { get; set; }
        public int? PartId { get; set; }
        public DateTime? CreatedAt{ get; set; }
        public DateTime? UpdatedAt{ get; set; }
        public string? CreatedBy{ get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? Date { get; set; }
        public string? Technician { get; set; }
        public string? Code { get; set; }
        public string? Notes { get; set; }
        public string? PartCode { get; set; }
        public int? VehicleId { get; set; }
        public decimal? CurrentOdometer { get; set; }
        public decimal? OdometerUpcomingMaintenance { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public string? VehicleVin { get; set; }
        public string? MakeName { get; set; }
        public string? ModelName { get; set; }
        public string? Plate { get; set; }
        public MaitenanceStatusEnum? Status { get; set; } = MaitenanceStatusEnum.NotStarted;
    }
}
