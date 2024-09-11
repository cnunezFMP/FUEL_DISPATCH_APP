using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FUEL_DISPATCH_API.DataAccess.Models
{
    public class Part
    {
        public int Id { get; set; }
        [Required] public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Code { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? CreatedBy { get; set; }
        public string? Manufacturer { get; set; }
        public int? CompanyId { get; set; }
        [Required] public decimal? MaintenanceOdometerInt { get; set; }
        [Required] public int MaintenanceMonthsInt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [JsonIgnore]
        public virtual ICollection<Maintenance> Maintenances { get; set; } = new List<Maintenance>();
        [JsonIgnore]
        public virtual Companies? Company { get; set; }
    }

}
